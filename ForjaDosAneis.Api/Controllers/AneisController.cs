using ForjaDosAneis.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AneisController : ControllerBase
{
    private readonly AnelDbContext _context;

    public AneisController(AnelDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Anel>>> GetAneis()
    {
        return await _context.Aneis.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Anel>> GetAnel(int id)
    {
        var anel = await _context.Aneis.FindAsync(id);
        if (anel == null) return NotFound();
        return anel;
    }

    [HttpPost]
    public async Task<ActionResult<Anel>> PostAnel(Anel anel)
    {
        if (!ValidarLimitesDeAneis(anel))
            return BadRequest("Limite de anéis para esta categoria foi excedido.");

        _context.Aneis.Add(anel);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAnel), new { id = anel.Id }, anel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAnel(int id, Anel anel)
    {
        if (id != anel.Id) return BadRequest();

        if (!ValidarLimitesDeAneis(anel))
            return BadRequest("Limite de anéis para esta categoria foi excedido.");

        _context.Entry(anel).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AnelExists(id)) return NotFound();
            else throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnel(int id)
    {
        var anel = await _context.Aneis.FindAsync(id);
        if (anel == null) return NotFound();

        _context.Aneis.Remove(anel);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool AnelExists(int id)
    {
        return _context.Aneis.Any(e => e.Id == id);
    }

    private bool ValidarLimitesDeAneis(Anel anel)
    {
        var limite = anel.Portador.ToLower() switch
        {
            "elfos" => 3,
            "anões" => 7,
            "homens" => 9,
            "sauron" => 1,
            _ => int.MaxValue
        };

        var totalAtual = _context.Aneis.Count(a => a.Portador.ToLower() == anel.Portador.ToLower());
        return totalAtual < limite;
    }
}
