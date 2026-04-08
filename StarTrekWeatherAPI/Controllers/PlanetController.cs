// Controllers/PlanetController.cs
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PlanetController : ControllerBase
{
    private static readonly List<Planet> Planets = new List<Planet>
    {
        new Planet
        {
            Name = "Earth",
            SolarSystem = "Sol",
            MaxTemp = 56.7f,
            MinTemp = -89.2f,
            Description = "A temperate world and home of the United Federation of Planets headquarters."
        },
        new Planet
        {
            Name = "Qo'noS",
            SolarSystem = "Qo'noS System",
            MaxTemp = 45.0f,
            MinTemp = -30.0f,
            Description = "The harsh homeworld of the Klingon Empire, known for its turbulent atmosphere."
        },
        new Planet
        {
            Name = "Vulcan",
            SolarSystem = "40 Eridani",
            MaxTemp = 67.0f,
            MinTemp = 0.0f,
            Description = "An arid, high-gravity desert world and home of the logical and disciplined Vulcan people."
        },
        new Planet
        {
            Name = "Romulus",
            SolarSystem = "128 Trianguli",
            MaxTemp = 40.0f,
            MinTemp = -10.0f,
            Description = "The secretive homeworld of the Romulan Star Empire, sister planet to Remus."
        }
    };

    [HttpGet]
    public IEnumerable<Planet> GetAll() => Planets;

    [HttpGet("{name}")]
    public ActionResult<Planet> GetByName(string name)
    {
        var planet = Planets.FirstOrDefault(p =>
            p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (planet == null) return NotFound();
        return planet;
    }
}