using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StarTrekWeather.Data;
using StarTrekWeather.Models;

public class CreateAccountModel : PageModel
{
    private readonly AppDbContext _db;

    public CreateAccountModel(AppDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public string Username { get; set; } = "";

    [BindProperty]
    public string Password { get; set; } = "";

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            ErrorMessage = "Username is required.";
            return Page();
        }

        if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6 || Password.Length > 20)
        {
            ErrorMessage = "Password must be 6-20 characters.";
            return Page();
        }

        var username = Username.Trim();

        var exists = _db.Users.Any(u => u.Username == username);
        if (exists)
        {
            ErrorMessage = "That username is already taken.";
            return Page();
        }

        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password)
        };

        _db.Users.Add(user);
        _db.SaveChanges();

        TempData["SuccessMessage"] = "Account created successfully. Please log in.";
        return RedirectToPage("/Login");
    }
}