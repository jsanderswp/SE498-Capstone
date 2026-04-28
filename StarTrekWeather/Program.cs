using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using StarTrekWeather.Data;
using StarTrekWeather.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<TempService>();
builder.Services.AddHttpClient("StarTrekWeatherAPI", client =>
{
    client.BaseAddress = new Uri("http://api:8080/");
});
//This was removed so I could run the project, I couldn't run it with these lines

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.MapRazorPages();
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}