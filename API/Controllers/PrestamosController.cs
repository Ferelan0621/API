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
    public class PrestamosController : ControllerBase
    {
        private readonly DBContext _context;

        public PrestamosController(DBContext context)
        {
            _context = context;
            
        }



        // GET: api/Prestamos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prestamos>>> GetPrestamos()
        {
            return await _context.Prestamos
                .Include(p => p.Laboratorio) 
                .Include(p => p.Usuario)     
                .Include(p => p.Encargado)   
                .ToListAsync();
        }

        // GET: api/Prestamos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prestamos>> GetPrestamos(int id)
        {
            // Nota: FindAsync() no soporta .Include(), así que usamos FirstOrDefaultAsync()
            var prestamo = await _context.Prestamos
                .Include(p => p.Laboratorio)
                .Include(p => p.Usuario)
                .Include(p => p.Encargado)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (prestamo == null)
            {
                return NotFound();
            }

            return prestamo;
        }

        // PUT: api/Prestamos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrestamos(int? id, Prestamos prestamos)
        {
            if (id != prestamos.ID)
            {
                return BadRequest();
            }

            _context.Entry(prestamos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrestamosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Prestamos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Prestamos>> PostPrestamos(Prestamos prestamos)
        {
            _context.Prestamos.Add(prestamos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrestamos", new { id = prestamos.ID }, prestamos);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Prestamos>>> GetPrestamosPorUsuario(int usuarioId)
        {
            var prestamos = await _context.Prestamos
                .Include(p => p.Laboratorio) // IMPORTANTE: Para que el XAML pueda pintar el NombreLaboratorio
                .Where(p => p.UsuarioID == usuarioId)
                .OrderByDescending(p => p.FechaSolicitud) // Ordenar los más recientes primero
                .ToListAsync();

            if (prestamos == null || !prestamos.Any())
            {
                return NotFound("No se encontraron préstamos para este usuario.");
            }

            return Ok(prestamos);
        }
        // DELETE: api/Prestamos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrestamos(int? id)
        {
            var prestamos = await _context.Prestamos.FindAsync(id);
            if (prestamos == null)
            {
                return NotFound();
            }

            _context.Prestamos.Remove(prestamos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrestamosExists(int? id)
        {
            return _context.Prestamos.Any(e => e.ID == id);
        }
    }
}