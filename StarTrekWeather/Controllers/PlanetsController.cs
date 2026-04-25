// Controllers/PlanetsController.cs
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using StarTrekWeather.Data;
using StarTrekWeather.Models;
using StarTrekWeather.Services;

namespace StarTrekWeather.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanetsController : ControllerBase
    {
        private readonly HttpClient _httpClient;
		private readonly TempService _tempService;

        public PlanetsController(IHttpClientFactory httpClientFactory, TempService tempService)
        {
            _httpClient = httpClientFactory.CreateClient("StarTrekWeatherAPI");
			_tempService = tempService;
        }

        // GET api/planets
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var planets = await _httpClient.GetFromJsonAsync<List<PlanetDto>>("api/planet");
            if (planets is null) return StatusCode(500, new { error = "Failed to retrieve planets." });
            return Ok(planets);
        }

        // GET api/planets/{name}
        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var response = await _httpClient.GetAsync($"api/planet/{name}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound(new { error = "Planet not found." });

            if (!response.IsSuccessStatusCode)
                return StatusCode(500, new { error = "Failed to retrieve planet." });

            var planet = await response.Content.ReadFromJsonAsync<PlanetDto>();
            if (planet is null) return StatusCode(500, new { error = "Failed to parse planet data." });
			var CurrentTemp = _tempService.GetCurrentTemp(planet.MaxTemp, planet.MinTemp);
            return Ok(new
            {
                planet.Name,
                planet.SolarSystem,
                planet.Description,
                planet.AtmosphericPressure,
                planet.MinTemp,
                planet.MaxTemp,
                CurrentTemp
            });
        }

        private class PlanetDto
        {
            public string Name { get; set; } = "";
            public string SolarSystem { get; set; } = "";
            public string Description { get; set; } = "";
            public float AtmosphericPressure { get; set; }
            public float MaxTemp { get; set; }
            public float MinTemp { get; set; }
        }
    }
}