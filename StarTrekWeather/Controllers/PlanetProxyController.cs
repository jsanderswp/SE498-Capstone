using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/planetproxy")]
public class PlanetProxyController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PlanetProxyController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(
            $"http://api:8080/api/planet/search?q={Uri.EscapeDataString(q)}");

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        return Content(content, "application/json");
    }
}