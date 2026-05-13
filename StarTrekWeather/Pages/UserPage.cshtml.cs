using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StarTrekWeather.Data;

public class UserPageModel : PageModel
{
    private readonly AppDbContext _db;

    public UserPageModel(AppDbContext db)
    {
        _db = db;
    }

    public string Username { get; private set; } = "";
    public string AppMode { get; private set; } = "startrek";
    public List<string> SavedPlanets { get; private set; } = new();
    public List<string> SavedPokemonLocations { get; private set; } = new();

    public IActionResult OnGet()
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrWhiteSpace(username))
        {
            return RedirectToPage("/Login");
        }

        LoadSavedItems(username);
        return Page();
    }

    public IActionResult OnPostRemove(string planetName)
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrWhiteSpace(username))
        {
            return RedirectToPage("/Login");
        }

        if (!string.IsNullOrWhiteSpace(planetName))
        {
            var savedPlanet = _db.UserPlanets.FirstOrDefault(up =>
                up.Username == username && up.PlanetName == planetName);

            if (savedPlanet != null)
            {
                _db.UserPlanets.Remove(savedPlanet);
                _db.SaveChanges();
            }
        }

        return RedirectToPage();
    }

    public IActionResult OnPostRemovePokemon(string locationName)
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrWhiteSpace(username))
        {
            return RedirectToPage("/Login");
        }

        if (!string.IsNullOrWhiteSpace(locationName))
        {
            var savedLocation = _db.UserPokemonLocations.FirstOrDefault(ul =>
                ul.Username == username && ul.LocationName == locationName);

            if (savedLocation != null)
            {
                _db.UserPokemonLocations.Remove(savedLocation);
                _db.SaveChanges();
            }
        }

        return RedirectToPage();
    }

    private void LoadSavedItems(string username)
    {
        Username = username;
        AppMode = HttpContext.Session.GetString("AppMode") ?? "startrek";
        SavedPlanets = _db.UserPlanets
            .Where(up => up.Username == username)
            .Select(up => up.PlanetName)
            .OrderBy(name => name)
            .ToList();
        SavedPokemonLocations = _db.UserPokemonLocations
            .Where(ul => ul.Username == username)
            .Select(ul => ul.LocationName)
            .OrderBy(name => name)
            .ToList();
    }
}
