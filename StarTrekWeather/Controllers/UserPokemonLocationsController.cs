using StarTrekWeather.Data;
using StarTrekWeather.Models;
using StarTrekWeather.Services;
using Microsoft.AspNetCore.Mvc;
 
namespace StarTrekWeather.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserPokemonLocationsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly TempService _tempService;
 
        public UserPokemonLocationsController(AppDbContext db, TempService tempService)
        {
            _db = db;
            _tempService = tempService;
        }
 
        // POST api/userpokemonlocations
        [HttpPost]
        public IActionResult AddUserPokemonLocation([FromBody] UserPokemonLocation userPokemonLocation)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == userPokemonLocation.Username);
            if (user == null)
                return NotFound("User not found.");
 
            var exists = _db.UserPokemonLocations.Any(ul =>
                ul.Username == userPokemonLocation.Username &&
                ul.LocationName == userPokemonLocation.LocationName);
 
            if (exists)
                return Conflict("Pokemon location already saved for this user.");
 
            _db.UserPokemonLocations.Add(userPokemonLocation);
            _db.SaveChanges();
 
            return Ok(new { message = "Pokemon location added successfully." });
        }
 
        // GET api/userpokemonlocations/{username}
        [HttpGet("{username}")]
        public IActionResult GetUserPokemonLocations(string username)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                return NotFound("User not found.");
 
            var locations = _db.UserPokemonLocations
                .Where(ul => ul.Username == username)
                .Select(ul => ul.LocationName)
                .ToList();
 
            return Ok(locations);
        }
 
        // DELETE api/userpokemonlocations
        [HttpDelete]
        public IActionResult RemoveUserPokemonLocation([FromBody] UserPokemonLocation userPokemonLocation)
        {
            var entry = _db.UserPokemonLocations.FirstOrDefault(ul =>
                ul.Username == userPokemonLocation.Username &&
                ul.LocationName == userPokemonLocation.LocationName);
 
            if (entry == null)
                return NotFound("User pokemon location not found.");
 
            _db.UserPokemonLocations.Remove(entry);
            _db.SaveChanges();
 
            return Ok(new { message = "Pokemon location removed successfully." });
        }
 
        // GET api/userpokemonlocations/location/{locationName}
        [HttpGet("location/{locationName}")]
        public IActionResult GetLocationUsers(string locationName)
        {
            var users = _db.UserPokemonLocations
                .Where(ul => ul.LocationName == locationName)
                .Select(ul => ul.Username)
                .ToList();
 
            return Ok(new { location = locationName, userCount = users.Count, users = users });
        }
 
        // GET api/userpokemonlocations/{username}/{locationName}/temp
        [HttpGet("{username}/{locationName}/temp")]
        public IActionResult GetLocationTemp(string username, string locationName)
        {
            var entry = _db.UserPokemonLocations.FirstOrDefault(ul =>
                ul.Username == username &&
                ul.LocationName == locationName);
 
            if (entry == null)
                return NotFound("User pokemon location not found.");
 
            // Pokemon location temps range from -20°C to 45°C
            var temp = _tempService.GetCurrentTemp(45f, -20f);
 
            return Ok(new { username, locationName, temperature = temp });
        }
 
        // POST api/userpokemonlocations/{username}/{locationName}/gyms
        [HttpPost("{username}/{locationName}/gyms")]
        public IActionResult AddGym(string username, string locationName, [FromBody] UserPokemonLocationGym gym)
        {
            var location = _db.UserPokemonLocations.FirstOrDefault(ul =>
                ul.Username == username &&
                ul.LocationName == locationName);
 
            if (location == null)
                return NotFound("User pokemon location not found.");
 
            var exists = _db.UserPokemonLocationGyms.Any(g =>
                g.Username == username &&
                g.LocationName == locationName &&
                g.GymName == gym.GymName);
 
            if (exists)
                return Conflict("Gym already saved for this location.");
 
            gym.Username = username;
            gym.LocationName = locationName;
 
            _db.UserPokemonLocationGyms.Add(gym);
            _db.SaveChanges();
 
            return Ok(new { message = "Gym added successfully." });
        }
 
        // GET api/userpokemonlocations/{username}/{locationName}/gyms
        [HttpGet("{username}/{locationName}/gyms")]
        public IActionResult GetGyms(string username, string locationName)
        {
            var location = _db.UserPokemonLocations.FirstOrDefault(ul =>
                ul.Username == username &&
                ul.LocationName == locationName);
 
            if (location == null)
                return NotFound("User pokemon location not found.");
 
            var gyms = _db.UserPokemonLocationGyms
                .Where(g => g.Username == username && g.LocationName == locationName)
                .Select(g => g.GymName)
                .ToList();
 
            return Ok(gyms);
        }
 
        // DELETE api/userpokemonlocations/{username}/{locationName}/gyms/{gymName}
        [HttpDelete("{username}/{locationName}/gyms/{gymName}")]
        public IActionResult RemoveGym(string username, string locationName, string gymName)
        {
            var entry = _db.UserPokemonLocationGyms.FirstOrDefault(g =>
                g.Username == username &&
                g.LocationName == locationName &&
                g.GymName == gymName);
 
            if (entry == null)
                return NotFound("Gym not found.");
 
            _db.UserPokemonLocationGyms.Remove(entry);
            _db.SaveChanges();
 
            return Ok(new { message = "Gym removed successfully." });
        }
    }
}