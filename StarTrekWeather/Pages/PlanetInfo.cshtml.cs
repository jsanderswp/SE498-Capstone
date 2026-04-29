using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StarTrekWeather.Data;
using StarTrekWeather.Models;

public class PlanetInfoModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppDbContext _db;

    public PlanetInfoModel(IHttpClientFactory httpClientFactory, AppDbContext db)
    {
        _httpClientFactory = httpClientFactory;
        _db = db;
    }

    public string PlanetName { get; set; } = "";
    public string MeanTemp { get; set; } = "";
    public string AtmosphericPressure { get; set; } = "";
    public string SolarSystem { get; set; } = "";
    public string Description { get; set; } = "";
    public bool IsLoggedIn { get; set; }
    public string? SaveMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(string planet)
    {
        return await LoadPlanetAsync(planet);
    }

    public async Task<IActionResult> OnPostSaveAsync(string planet)
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(planet))
            return RedirectToPage("/Login");

        var exists = _db.UserPlanets.Any(up => up.Username == username && up.PlanetName == planet);
        if (!exists)
        {
            _db.UserPlanets.Add(new UserPlanet
            {
                Username = username,
                PlanetName = planet
            });

            _db.SaveChanges();
            SaveMessage = "Planet saved successfully.";
        }
        else
        {
            SaveMessage = "Planet already saved.";
        }

        return await LoadPlanetAsync(planet);
    }

    private async Task<IActionResult> LoadPlanetAsync(string planet)
    {
        if (string.IsNullOrWhiteSpace(planet))
            return RedirectToPage("/Index");

        IsLoggedIn = !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Username"));

        try
        {
            var client = _httpClientFactory.CreateClient("StarTrekWeatherAPI");
            var response = await client.GetAsync($"api/planet/{Uri.EscapeDataString(planet)}");

            if (!response.IsSuccessStatusCode)
                return RedirectToPage("/Index");

            var result = await response.Content.ReadFromJsonAsync<PlanetDto>();
            if (result is null)
                return RedirectToPage("/Index");

            var currentTemp = (result.MaxTemp + result.MinTemp) / 2f;

            PlanetName = result.Name;
            MeanTemp = $"{currentTemp:0.#}°C";
            AtmosphericPressure = result.AtmosphericPressure.ToString("0.#");
            SolarSystem = result.SolarSystem;
            Description = result.Description;

            return Page();
        }
        catch
        {
            return RedirectToPage("/Index");
        }
    }

    private class PlanetDto
    {
        public string Name { get; set; } = "";
        public float AtmosphericPressure { get; set; }
        public float MaxTemp { get; set; }
        public float MinTemp { get; set; }
        public string SolarSystem { get; set; } = "";
        public string Description { get; set; } = "";
    }
}
