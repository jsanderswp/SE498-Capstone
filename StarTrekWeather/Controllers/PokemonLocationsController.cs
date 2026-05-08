namespace StarTrekWeather.Models;

public class PokemonLocationsController
{
    private readonly HttpClient _client;

    public PokemonLocationsController(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("ExternalApi");
    }
    
    [HttpGet("locations")]
    public async Task<IActionResult> GetLocations()
    {
        var response = await _client.GetAsync("/Locations");
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);
        return Ok(await response.Content.ReadAsStringAsync());
    }
    
    
}

