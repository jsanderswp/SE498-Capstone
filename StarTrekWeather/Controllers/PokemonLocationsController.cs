using Microsoft.AspNetCore.Mvc;
using StarTrekWeather.Data;
using StarTrekWeather.Models;
using StarTrekWeather.Services;
using System.Net;
 
namespace StarTrekWeather.Controllers;
 
[ApiController]
[Route("[controller]")]
public class PokemonLocationsController : ControllerBase
{
    private readonly HttpClient _client;
    private readonly AppDbContext _db;
    private readonly TempService _tempService;
 
    public PokemonLocationsController(IHttpClientFactory httpClientFactory, AppDbContext db, TempService tempService)
    {
        _client = httpClientFactory.CreateClient("PokemonLocationsAPI");
        _db = db;
        _tempService = tempService;
    }
 
    // -------------------------
    // External API proxy routes
    // -------------------------
 
    // GET pokemonlocations/locations
    [HttpGet("locations")]
    public async Task<IActionResult> GetLocations()
    {
        var response = await _client.GetAsync("/Locations");
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);
        return Ok(await response.Content.ReadAsStringAsync());
    }
 
    // GET pokemonlocations/locations/{locationId}/buildings
    [HttpGet("locations/{locationId}/buildings")]
    public async Task<IActionResult> GetBuildings(int locationId)
    {
        var response = await _client.GetAsync($"/locations/{locationId}/buildings");
        if (response.StatusCode == HttpStatusCode.NotFound) return NotFound();
        return Ok(await response.Content.ReadAsStringAsync());
    }
 
    // GET pokemonlocations/locations/{locationId}/buildings/{buildingId}
    [HttpGet("locations/{locationId}/buildings/{buildingId}")]
    public async Task<IActionResult> GetBuilding(int locationId, int buildingId)
    {
        var response = await _client.GetAsync($"/locations/{locationId}/buildings/{buildingId}");
        if (response.StatusCode == HttpStatusCode.NotFound) return NotFound();
        return Ok(await response.Content.ReadAsStringAsync());
    }
 
    // GET pokemonlocations/gyms
    [HttpGet("gyms")]
    public async Task<IActionResult> GetGyms()
    {
        var response = await _client.GetAsync("/gyms");
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);
        return Ok(await response.Content.ReadAsStringAsync());
    }
 
    // GET pokemonlocations/gyms/{gymId}
    [HttpGet("gyms/{gymId}")]
    public async Task<IActionResult> GetGym(int gymId)
    {
        var response = await _client.GetAsync($"/gyms/{gymId}");
        if (response.StatusCode == HttpStatusCode.NotFound) return NotFound();
        return Ok(await response.Content.ReadAsStringAsync());
    }
 
    // -------------------------
    // User saved location routes
    // -------------------------
 
    // POST pokemonlocations/users
    [HttpPost("users")]
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
 
    // GET pokemonlocations/users/{username}
    [HttpGet("users/{username}")]
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
 
    // DELETE pokemonlocations/users
    [HttpDelete("users")]
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
 
    // GET pokemonlocations/users/location/{locationName}
    [HttpGet("users/location/{locationName}")]
    public IActionResult GetLocationUsers(string locationName)
    {
        var users = _db.UserPokemonLocations
            .Where(ul => ul.LocationName == locationName)
            .Select(ul => ul.Username)
            .ToList();
 
        return Ok(new { location = locationName, userCount = users.Count, users = users });
    }
 
    // GET pokemonlocations/users/{username}/{locationName}/temp
    [HttpGet("users/{username}/{locationName}/temp")]
    public IActionResult GetLocationTemp(string username, string locationName)
    {
        var entry = _db.UserPokemonLocations.FirstOrDefault(ul =>
            ul.Username == username &&
            ul.LocationName == locationName);
 
        if (entry == null)
            return NotFound("User pokemon location not found.");
 
        var temp = _tempService.GetCurrentTemp(45f, -20f);
 
        return Ok(new { username, locationName, temperature = temp });
    }
 
    // -------------------------
    // User saved gym routes
    // -------------------------
 
    // POST pokemonlocations/users/{username}/{locationName}/gyms
    [HttpPost("users/{username}/{locationName}/gyms")]
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
 
    // GET pokemonlocations/users/{username}/{locationName}/gyms
    [HttpGet("users/{username}/{locationName}/gyms")]
    public IActionResult GetUserGyms(string username, string locationName)
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
 
    // DELETE pokemonlocations/users/{username}/{locationName}/gyms/{gymName}
    [HttpDelete("users/{username}/{locationName}/gyms/{gymName}")]
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
 
    // -------------------------
    // User saved building routes
    // -------------------------
 
    // POST pokemonlocations/users/{username}/{locationName}/buildings
    [HttpPost("users/{username}/{locationName}/buildings")]
    public IActionResult AddBuilding(string username, string locationName, [FromBody] UserPokemonLocationBuilding building)
    {
        var location = _db.UserPokemonLocations.FirstOrDefault(ul =>
            ul.Username == username &&
            ul.LocationName == locationName);
 
        if (location == null)
            return NotFound("User pokemon location not found.");
 
        var exists = _db.UserPokemonLocationBuildings.Any(b =>
            b.Username == username &&
            b.LocationName == locationName &&
            b.BuildingName == building.BuildingName);
 
        if (exists)
            return Conflict("Building already saved for this location.");
 
        building.Username = username;
        building.LocationName = locationName;
 
        _db.UserPokemonLocationBuildings.Add(building);
        _db.SaveChanges();
 
        return Ok(new { message = "Building added successfully." });
    }
 
    // GET pokemonlocations/users/{username}/{locationName}/buildings
    [HttpGet("users/{username}/{locationName}/buildings")]
    public IActionResult GetUserBuildings(string username, string locationName)
    {
        var location = _db.UserPokemonLocations.FirstOrDefault(ul =>
            ul.Username == username &&
            ul.LocationName == locationName);
 
        if (location == null)
            return NotFound("User pokemon location not found.");
 
        var buildings = _db.UserPokemonLocationBuildings
            .Where(b => b.Username == username && b.LocationName == locationName)
            .Select(b => b.BuildingName)
            .ToList();
 
        return Ok(buildings);
    }
 
    // DELETE pokemonlocations/users/{username}/{locationName}/buildings/{buildingName}
    [HttpDelete("users/{username}/{locationName}/buildings/{buildingName}")]
    public IActionResult RemoveBuilding(string username, string locationName, string buildingName)
    {
        var entry = _db.UserPokemonLocationBuildings.FirstOrDefault(b =>
            b.Username == username &&
            b.LocationName == locationName &&
            b.BuildingName == buildingName);
 
        if (entry == null)
            return NotFound("Building not found.");
 
        _db.UserPokemonLocationBuildings.Remove(entry);
        _db.SaveChanges();
 
        return Ok(new { message = "Building removed successfully." });
    }
}