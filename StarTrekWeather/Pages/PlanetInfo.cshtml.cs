using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class PlanetInfoModel : PageModel
{
    public string PlanetName { get; set; } = "";
    public string MeanTemp { get; set; } = "";
    public string AtmosphericPressure { get; set; } = "";
    public string SolarSystem { get; set; } = "";
    public string Description { get; set; } = "";

    public async Task<IActionResult> OnGetAsync(string planet)
    {
        if (string.IsNullOrWhiteSpace(planet))
            return RedirectToPage("/Index");

        using var client = new HttpClient();
        var result = await client.GetFromJsonAsync<PlanetDto>(
            $"http://api:8080/api/planet/{Uri.EscapeDataString(planet)}");

        if (result is null)
            return RedirectToPage("/Index");

        PlanetName = result.Name;
        MeanTemp = result.MeanTemp;
        AtmosphericPressure = result.AtmosphericPressure;
        SolarSystem = result.SolarSystem;
        Description = result.Description;

        return Page();
    }

    private class PlanetDto
    {
        public string Name { get; set; } = "";
        public string MeanTemp { get; set; } = "";
        public string AtmosphericPressure { get; set; } = "";
        public string SolarSystem { get; set; } = "";
        public string Description { get; set; } = "";
    }
}