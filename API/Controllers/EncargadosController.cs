using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;

[Route("api/[controller]")]
[ApiController]
public class EncargadosController : ControllerBase
{
    private readonly DBContext _context;
    public EncargadosController(DBContext context)
    {
        _context = context;
    }

    // GET: api/Encargados
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Encargados>>> GetEncargados()
    {
        return await _context.Encargados.ToListAsync();
    }

    // GET: api/Encargados/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Encargados>> GetEncargados(int id)
    {
        var encargados = await _context.Encargados.FindAsync(id);

        if (encargados == null)
        {
            return NotFound();
        }

        return encargados;
    }

    // PUT: api/Encargados/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEncargados(int? id, Encargados encargados)
    {
        if (id != encargados.ID)
        {
            return BadRequest();
        }

        _context.Entry(encargados).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EncargadosExists(id))
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

    // POST: api/Encargados
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Encargados>> PostEncargados(Encargados encargados)
    {
        _context.Encargados.Add(encargados);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetEncargados", new { id = encargados.ID }, encargados);
    }

    // DELETE: api/Encargados/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEncargados(int? id)
    {
        var encargados = await _context.Encargados.FindAsync(id);
        if (encargados == null)
        {
            return NotFound();
        }

        _context.Encargados.Remove(encargados);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EncargadosExists(int? id)
    {
        return _context.Encargados.Any(e => e.ID == id);
    }
}
