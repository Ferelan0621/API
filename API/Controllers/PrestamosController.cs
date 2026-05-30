using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;

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
        return await _context.Prestamos.ToListAsync();
    }

    // GET: api/Prestamos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Prestamos>> GetPrestamos(int id)
    {
        var prestamos = await _context.Prestamos.FindAsync(id);

        if (prestamos == null)
        {
            return NotFound();
        }

        return prestamos;
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
