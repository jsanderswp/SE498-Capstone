using Microsoft.AspNetCore.Mvc.RazorPages;

public class PlanetInfoModel : PageModel
{
    public string PlanetName { get; set; } = "";
    public string MeanTemp { get; set; } = "";
    public string AtmosphericPressure { get; set; } = "";
    public string SolarSystem { get; set; } = "";

    public void OnGet()
    {
        PlanetName = "Trill (Alpha Quadrant)";
        MeanTemp = "297 K (24°C / 75°F)";
        AtmosphericPressure = "1.012 bar";
        SolarSystem = "Trill System";
    }
}