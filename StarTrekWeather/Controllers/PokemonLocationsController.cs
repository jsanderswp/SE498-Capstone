namespace StarTrekWeather.Models;

public class PokemonLocationsController
{
    private readonly string _apiKey;

    public MyController(IConfiguration configuration)
    {
        _apiKey = configuration["MY_API_KEY"];
    }
}

