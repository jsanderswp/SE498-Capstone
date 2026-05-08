namespace StarTrekWeather.Models;

public class PokemonLocationsController
{
    private readonly HttpClient _client;

    public PokemonLocationsController(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("ExternalApi");
    }
    
    
}

