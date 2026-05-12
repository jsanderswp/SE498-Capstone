namespace StarTrekWeather.Models;
public class User
{ 
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public ICollection<UserPlanet> UserPlanets { get; set; } = new List<UserPlanet>(); //planets collection for users
    public ICollection<UserPokemonLocation> UserPokemonLocations { get; set; } = new List<UserPokemonLocation>(); //pokemon locations collection for users
}
