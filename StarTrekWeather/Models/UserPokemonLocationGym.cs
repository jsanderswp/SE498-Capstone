namespace StarTrekWeather.Models;
 
public class UserPokemonLocationGym
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string LocationName { get; set; } = null!;
    public string GymName { get; set; } = null!;
 
    public UserPokemonLocation UserPokemonLocation { get; set; } = null!;
}