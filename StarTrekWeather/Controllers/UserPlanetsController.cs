using StarTrekWeather.Data;
using StarTrekWeather.Models;
using Microsoft.AspNetCore.Mvc;

namespace StarTrekWeather.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserPlanetsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UserPlanetsController(AppDbContext db)
        {
            _db = db;
        }

        // POST api/userplanets
        [HttpPost]
        public IActionResult AddUserPlanet([FromBody] UserPlanet userPlanet)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == userPlanet.Username);
            if (user == null)
                return NotFound("User not found.");

            var exists = _db.UserPlanets.Any(up =>
                up.Username == userPlanet.Username &&
                up.PlanetName == userPlanet.PlanetName);

            if (exists)
                return Conflict("Planet already saved for this user.");

            _db.UserPlanets.Add(userPlanet);
            _db.SaveChanges();

            return Ok(new { message = "Planet added successfully." });
        }

        // GET api/userplanets/{username}
        [HttpGet("{username}")]
        public IActionResult GetUserPlanets(string username)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                return NotFound("User not found.");

            var planets = _db.UserPlanets
                .Where(up => up.Username == username)
                .Select(up => up.PlanetName)
                .ToList();

            return Ok(planets);
        }

        // DELETE api/userplanets
        [HttpDelete]
        public IActionResult RemoveUserPlanet([FromBody] UserPlanet userPlanet)
        {
            var entry = _db.UserPlanets.FirstOrDefault(up =>
                up.Username == userPlanet.Username &&
                up.PlanetName == userPlanet.PlanetName);

            if (entry == null)
                return NotFound("User planet not found.");

            _db.UserPlanets.Remove(entry);
            _db.SaveChanges();

            return Ok(new { message = "Planet removed successfully." });
        }
        
        // GET api/userplanets/planet/{planetName}
        [HttpGet("planet/{planetName}")]
        public IActionResult GetPlanetUsers(string planetName)
        {
            var users = _db.UserPlanets
                .Where(up => up.PlanetName == planetName)
                .Select(up => up.Username)
                .ToList();

            return Ok(new { planet = planetName, userCount = users.Count, users = users });
        }
    }
}