namespace StarTrekWeather.Models;
 
public class UserPokemonLocation
{
    public string Username { get; set; } = null!;
    public string LocationName { get; set; } = null!;
 
    public User User { get; set; } = null!;
    public ICollection<UserPokemonLocationBuilding> Buildings { get; set; } = new List<UserPokemonLocationBuilding>();
    public ICollection<UserPokemonLocationGym> Gyms { get; set; } = new List<UserPokemonLocationGym>();
}