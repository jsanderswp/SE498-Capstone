namespace StarTrekWeather.models;

public class UserPlanet
{
    public string Username { get; set; } = null!;
    public string PlanetName { get; set; } = null!;

    public User User { get; set; } = null!;
}