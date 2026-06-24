using API.Data;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaboratoriosController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly NotificadorLaboratorios _notificador;

        public LaboratoriosController(DBContext context, NotificadorLaboratorios notificador)
        {
            _context = context;
            _notificador = notificador;
        }

        // --- ENDPOINT SSE ---
        [HttpGet("stream")]
        public async Task GetStream(CancellationToken ct)
        {
            // 1. Corrección en la asignación de cabeceras para evitar excepciones "Key already exists"
            Response.ContentType = "text/event-stream";
            Response.Headers["Cache-Control"] = "no-cache";
            Response.Headers["Connection"] = "keep-alive";

            // 2. Corrección del TaskCompletionSource para compatibilidad de versiones
            var tcs = new TaskCompletionSource<bool>();
            ct.Register(() => tcs.TrySetResult(true));

            // 3. Corrección a async void con manejo de excepciones y await
            async void EnviarActualizacion(string json)
            {
                try
                {
                    await Response.WriteAsync($"data: {json}\n\n");
                    await Response.Body.FlushAsync();
                }
                catch (Exception)
                {
                    // Si el cliente se desconectó justo antes de enviar, ignoramos el error
                    // para no tumbar la aplicación.
                }
            }

            _notificador.OnLaboratorioActualizado += EnviarActualizacion;

            try
            {
                await tcs.Task;
            }
            finally
            {
                _notificador.OnLaboratorioActualizado -= EnviarActualizacion;
            }
        }

        // --- OPERACIONES REST ---

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Laboratorios>>> GetLaboratorios()
        {
            return await _context.Laboratorios.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Laboratorios>> GetLaboratorios(int id)
        {
            var laboratorio = await _context.Laboratorios.FindAsync(id);
            if (laboratorio == null) return NotFound();
            return laboratorio;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLaboratorios(int? id, Laboratorios laboratorios)
        {
            if (id != laboratorios.ID) return BadRequest();

            _context.Entry(laboratorios).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await NotificarCambios(); // Notificar en tiempo real
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LaboratoriosExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Laboratorios>> PostLaboratorios(Laboratorios laboratorios)
        {
            _context.Laboratorios.Add(laboratorios);
            await _context.SaveChangesAsync();

            await NotificarCambios(); // Notificar en tiempo real
            return CreatedAtAction("GetLaboratorios", new { id = laboratorios.ID }, laboratorios);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLaboratorios(int? id)
        {
            var laboratorio = await _context.Laboratorios.FindAsync(id);
            if (laboratorio == null) return NotFound();

            _context.Laboratorios.Remove(laboratorio);
            await _context.SaveChangesAsync();

            await NotificarCambios(); // Notificar en tiempo real
            return NoContent();
        }

        // --- MÉTODOS PRIVADOS ---

        private async Task NotificarCambios()
        {
            var lista = await _context.Laboratorios.ToListAsync();
            var json = JsonSerializer.Serialize(lista, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            _notificador.NotificarCambio(json);
        }

        private bool LaboratoriosExists(int? id)
        {
            return _context.Laboratorios.Any(e => e.ID == id);
        }
    }
}