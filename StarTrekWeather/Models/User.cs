namespace StarTrekWeather.models;
public class User
{ 
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public ICollection<UserPlanet> UserPlanets { get; set; } = new List<UserPlanet>(); //planets collection for users
}
