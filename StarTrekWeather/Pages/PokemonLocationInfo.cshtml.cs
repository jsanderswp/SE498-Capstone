using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StarTrekWeather.Data;
using StarTrekWeather.Models;
using StarTrekWeather.Services;

public class PokemonLocationInfoModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppDbContext _db;
    private readonly TempService _tempService;

    public PokemonLocationInfoModel(IHttpClientFactory httpClientFactory, AppDbContext db, TempService tempService)
    {
        _httpClientFactory = httpClientFactory;
        _db = db;
        _tempService = tempService;
    }

    public int LocationId { get; private set; }
    public string LocationName { get; private set; } = "";
    public string CurrentTemp { get; private set; } = "";
    public string Description { get; private set; } = "";
    public bool IsLoggedIn { get; private set; }
    public string? SaveMessage { get; private set; }
    public List<string> Buildings { get; private set; } = new();
    public List<string> Gyms { get; private set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        return await LoadLocationAsync(id);
    }

    public async Task<IActionResult> OnPostSaveAsync(int id)
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrWhiteSpace(username) || id <= 0)
            return RedirectToPage("/Login");

        var client = _httpClientFactory.CreateClient("PokemonLocationsAPI");
        var response = await client.GetAsync($"/Locations/{id}");
        if (!response.IsSuccessStatusCode)
            return RedirectToPage("/Index");

        var location = await ParseAsync<LocationDto>(response);
        if (location is null)
            return RedirectToPage("/Index");

        var exists = _db.UserPokemonLocations.Any(ul =>
            ul.Username == username && ul.LocationName == location.Name);

        if (!exists)
        {
            _db.UserPokemonLocations.Add(new UserPokemonLocation
            {
                Username = username,
                LocationName = location.Name
            });
            _db.SaveChanges();
            SaveMessage = "Location saved successfully.";
        }
        else
        {
            SaveMessage = "Location already saved.";
        }

        return await LoadLocationAsync(id);
    }

    private async Task<IActionResult> LoadLocationAsync(int id)
    {
        if (id <= 0)
            return RedirectToPage("/Index");

        IsLoggedIn = !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Username"));

        try
        {
            var client = _httpClientFactory.CreateClient("PokemonLocationsAPI");

            var locationResponse = await client.GetAsync($"/Locations/{id}");
            if (!locationResponse.IsSuccessStatusCode)
                return RedirectToPage("/Index");

            var buildingsResponse = await client.GetAsync($"/locations/{id}/buildings");
            if (!buildingsResponse.IsSuccessStatusCode)
                return RedirectToPage("/Index");

            var location = await ParseAsync<LocationDto>(locationResponse);
            var buildings = await ParseAsync<List<BuildingDto>>(buildingsResponse) ?? new List<BuildingDto>();

            if (location is null)
                return RedirectToPage("/Index");

            LocationId = location.LocationId;
            LocationName = location.Name;
            Description = string.IsNullOrWhiteSpace(location.Description)
                ? "A notable location in the Pokemon world."
                : location.Description;

            var temp = _tempService.GetCurrentTemp(95f, 35f);
            CurrentTemp = $"{temp:0.#}°F";
            Buildings = buildings.Select(b => b.Name).Distinct().OrderBy(name => name).ToList();
            Gyms = buildings
                .Where(b => b.Gym is not null)
                .Select(b => $"{b.Gym!.GymLeader} ({b.Gym.GymType})")
                .Distinct()
                .OrderBy(name => name)
                .ToList();

            return Page();
        }
        catch
        {
            return RedirectToPage("/Index");
        }
    }

    private static async Task<T?> ParseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
            return default;

        if (content.StartsWith("\""))
        {
            content = System.Text.Json.JsonSerializer.Deserialize<string>(content) ?? "";
        }

        return System.Text.Json.JsonSerializer.Deserialize<T>(content,
            new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }

    private class LocationDto
    {
        public int LocationId { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
    }

    private class BuildingDto
    {
        public string Name { get; set; } = "";
        public GymDto? Gym { get; set; }
    }

    private class GymDto
    {
        public string GymType { get; set; } = "";
        public string GymLeader { get; set; } = "";
    }
}
