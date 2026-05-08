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
builder.Services.AddScoped<TempService>();

builder.Services.AddHttpClient("StarTrekWeatherAPI", client =>
{
    client.BaseAddress = new Uri("http://api:8080/");

    var credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("admin:password123"));
    client.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
});

builder.Services.AddHttpClient("PokemonLocationsAPI", client =>
{
    client.BaseAddress = new Uri("http://host.containers.internal:8081");
    client.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", builder.Configuration["EXTERNAL_API_KEY"]);
});


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapControllers();
app.MapRazorPages();

app.Run();
