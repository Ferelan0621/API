using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;

[Route("api/[controller]")]
[ApiController]
public class LaboratoriosController : ControllerBase
{
    private readonly DBContext _context;
    public LaboratoriosController(DBContext context)
    {
        _context = context;
    }

    // GET: api/Laboratorios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Laboratorios>>> GetLaboratorios()
    {
        return await _context.Laboratorios.ToListAsync();
    }

    // GET: api/Laboratorios/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Laboratorios>> GetLaboratorios(int id)
    {
        var laboratorios = await _context.Laboratorios.FindAsync(id);

        if (laboratorios == null)
        {
            return NotFound();
        }

        return laboratorios;
    }

    // PUT: api/Laboratorios/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLaboratorios(int? id, Laboratorios laboratorios)
    {
        if (id != laboratorios.ID)
        {
            return BadRequest();
        }

        _context.Entry(laboratorios).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LaboratoriosExists(id))
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

    // POST: api/Laboratorios
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Laboratorios>> PostLaboratorios(Laboratorios laboratorios)
    {
        _context.Laboratorios.Add(laboratorios);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetLaboratorios", new { id = laboratorios.ID }, laboratorios);
    }

    // DELETE: api/Laboratorios/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLaboratorios(int? id)
    {
        var laboratorios = await _context.Laboratorios.FindAsync(id);
        if (laboratorios == null)
        {
            return NotFound();
        }

        _context.Laboratorios.Remove(laboratorios);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool LaboratoriosExists(int? id)
    {
        return _context.Laboratorios.Any(e => e.ID == id);
    }
}
