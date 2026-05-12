namespace StarTrekWeather.Models;
 
public class UserPokemonLocation
{
    public string Username { get; set; } = null!;
    public string LocationName { get; set; } = null!;
 
    public User User { get; set; } = null!;
}