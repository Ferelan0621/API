using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using Shared.Models;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly DBContext _context;
    public UsuariosController(DBContext context)
    {
        _context = context;
    }

    // GET: api/Usuarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuarios>>> GetUsuarios()
    {
        return await _context.Usuarios.ToListAsync();
    }

    // GET: api/Usuarios/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Usuarios>> GetUsuarios(int id)
    {
        var usuarios = await _context.Usuarios.FindAsync(id);

        if (usuarios == null)
        {
            return NotFound();
        }

        return usuarios;
    }

    // PUT: api/Usuarios/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUsuarios(int? id, Usuarios usuarios)
    {
        if (id != usuarios.ID)
        {
            return BadRequest();
        }

        _context.Entry(usuarios).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UsuariosExists(id))
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

    // POST: api/Usuarios
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Usuarios>> PostUsuarios(Usuarios usuarios)
    {
        _context.Usuarios.Add(usuarios);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUsuarios", new { id = usuarios.ID }, usuarios);
    }

    // DELETE: api/Usuarios/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuarios(int? id)
    {
        var usuarios = await _context.Usuarios.FindAsync(id);
        if (usuarios == null)
        {
            return NotFound();
        }

        _context.Usuarios.Remove(usuarios);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UsuariosExists(int? id)
    {
        return _context.Usuarios.Any(e => e.ID == id);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.ClaveISSEMYM) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Faltan ingresar datos requeridos.");
        }

        // Buscamos en la base de datos de SQL Server
        var usuarios = await _context.Usuarios
            .FirstOrDefaultAsync(e => e.ClaveISSEMYM == request.ClaveISSEMYM && e.Password == request.Password);

        if (usuarios == null)

        {
            return Unauthorized("La clave o la contraseña son incorrectas.");
        }

        // Si todo está bien, regresas el usuario (o un token en el futuro)
        return Ok(new { Message = "Login exitoso", usuarioID = usuarios.ID, nombreUser = usuarios.Nombre});
    }
}
