namespace StarTrekWeather.Models;
 
public class UserPokemonLocationBuilding
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string LocationName { get; set; } = null!;
    public string BuildingName { get; set; } = null!;
 
    public UserPokemonLocation UserPokemonLocation { get; set; } = null!;
}