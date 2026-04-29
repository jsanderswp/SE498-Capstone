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
    public List<string> SavedPlanets { get; private set; } = new();

    public IActionResult OnGet()
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrWhiteSpace(username))
        {
            return RedirectToPage("/Login");
        }

        Username = username;
        SavedPlanets = _db.UserPlanets
            .Where(up => up.Username == username)
            .Select(up => up.PlanetName)
            .OrderBy(name => name)
            .ToList();

        return Page();
    }
}