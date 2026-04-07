using StarTrekWeather.Data;
using StarTrekWeather.Models;
using Microsoft.AspNetCore.Mvc;

namespace StarTrekWeather.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UsersController(AppDbContext db)
        {
            _db = db;
        }

        // POST api/users/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 6)
                return BadRequest("Password must be at least 6 characters.");

            // Hash password
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _db.Users.Add(user);
            _db.SaveChanges();

            return Ok(new { message = "User created successfully" });
        }

        // POST api/users/login
        public class LoginRequest
        {
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == loginRequest.Username);

            if (user == null)
                return Unauthorized("User not found");

            bool isValid = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password);

            if (!isValid)
                return Unauthorized("Invalid password");

            return Ok(new { message = "Login successful", userID = user.UserID });
        }
    }
}