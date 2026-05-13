using Microsoft.AspNetCore.Mvc;

namespace StarTrekWeather.Controllers
{
    [ApiController]
    [Route("api/appmode")]
    public class AppModeController : ControllerBase
    {
        [HttpPost]
        public IActionResult SetMode([FromBody] AppModeRequest request)
        {
            var mode = request.Mode?.ToLowerInvariant();
            if (mode != "startrek" && mode != "pokemon")
            {
                return BadRequest(new { error = "Invalid mode." });
            }

            HttpContext.Session.SetString("AppMode", mode);
            return Ok(new { mode });
        }

        public class AppModeRequest
        {
            public string? Mode { get; set; }
        }
    }
}