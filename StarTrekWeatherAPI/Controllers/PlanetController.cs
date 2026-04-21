// Controllers/PlanetController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class PlanetController : ControllerBase
{
    private readonly ApiDbContext _context;

    public PlanetController(ApiDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Planet>> GetAll()
    {
        return await _context.Planets.ToListAsync();
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<Planet>> GetByName(string name)
    {
        var planet = await _context.Planets.FirstOrDefaultAsync(p =>
            p.Name.ToLower() == name.ToLower());

        if (planet == null) return NotFound();
        return planet;
    }
}