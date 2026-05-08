using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;

namespace StarTrekWeather.Models;

public class PokemonLocationsController
{
    private readonly HttpClient _client;

    public PokemonLocationsController(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("PokemonLocaationsAPI");
    }
    
    [HttpGet("locations")]
    public async Task<IActionResult> GetLocations()
    {
        var response = await _client.GetAsync("/Locations");
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);
        return Ok(await response.Content.ReadAsStringAsync());
    }
    
    [HttpGet("locations/{locationId}/buildings")]
    public async Task<IActionResult> GetBuildings(int locationId)
    {
        var response = await _client.GetAsync($"/locations/{locationId}/buildings");
        if (response.StatusCode == HttpStatusCode.NotFound) return NotFound();
        return Ok(await response.Content.ReadAsStringAsync());
    }

    [HttpGet("locations/{locationId}/buildings/{buildingId}")]
    public async Task<IActionResult> GetBuilding(int locationId, int buildingId)
    {
        var response = await _client.GetAsync($"/locations/{locationId}/buildings/{buildingId}");
        if (response.StatusCode == HttpStatusCode.NotFound) return NotFound();
        return Ok(await response.Content.ReadAsStringAsync());
    }
    
    [HttpGet("gyms")]
    public async Task<IActionResult> GetGyms()
    {
        var response = await _client.GetAsync("/gyms");
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);
        return Ok(await response.Content.ReadAsStringAsync());
    }

    [HttpGet("gyms/{gymId}")]
    public async Task<IActionResult> GetGym(int gymId)
    {
        var response = await _client.GetAsync($"/gyms/{gymId}");
        if (response.StatusCode == HttpStatusCode.NotFound) return NotFound();
        return Ok(await response.Content.ReadAsStringAsync());
    }
}

