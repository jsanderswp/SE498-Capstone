using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class PlanetInfoModel : PageModel
{
    public string PlanetName { get; set; } = "";
    public string MeanTemp { get; set; } = "";
    public string AtmosphericPressure { get; set; } = "";
    public string SolarSystem { get; set; } = "";
    public string Description {get; set;} = "";

    public async Task OnGetAsync()
    {
        using var client = new HttpClient();
        var planet = await client.GetFromJsonAsync<PlanetDto>("http://localhost:5166/api/planet/Earth");

        if (planet is null)
        {
            PlanetName = "Unknown";
            MeanTemp = "Unknown";
            AtmosphericPressure = "Unknown";
            SolarSystem = "Unknown";
            Description = "Unknown";
            return;
        }

        PlanetName = planet.Name;
        MeanTemp = planet.MeanTemp;
        Description = planet.Description;
        AtmosphericPressure = planet.AtmosphericPressure;
        SolarSystem = planet.SolarSystem;
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