// Controllers/PlanetController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class PlanetController : ControllerBase
{
    private readonly ApiDbContext _context;
    private readonly string _publicBaseUrl;

    public PlanetController(ApiDbContext context, IConfiguration configuration)
    {
        _context = context;
        _publicBaseUrl = (configuration["PublicBaseUrl"] ?? "").TrimEnd('/');
    }

    [HttpGet]
    public async Task<IEnumerable<Planet>> GetAll()
    {
        var planets = await _context.Planets.AsNoTracking().ToListAsync();
        foreach (var p in planets) p.ImageUrl = ToAbsoluteImageUrl(p.ImageUrl);
        return planets;
    }

    [HttpGet("search")]
    public IEnumerable<Planet> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q)) return Enumerable.Empty<Planet>();

        var planets = _context.Planets
            .AsNoTracking()
            .Where(p => p.Name.ToLower().Contains(q.ToLower()))
            .Take(5)
            .ToList();
        foreach (var p in planets) p.ImageUrl = ToAbsoluteImageUrl(p.ImageUrl);
        return planets;
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<Planet>> GetByName(string name)
    {
        var planet = await _context.Planets
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());

        if (planet == null) return NotFound();
        planet.ImageUrl = ToAbsoluteImageUrl(planet.ImageUrl);
        return planet;
    }

    private string ToAbsoluteImageUrl(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl)) return imageUrl;
        if (imageUrl.StartsWith("http://") || imageUrl.StartsWith("https://")) return imageUrl;
        return $"{_publicBaseUrl}{imageUrl}";
    }
}
