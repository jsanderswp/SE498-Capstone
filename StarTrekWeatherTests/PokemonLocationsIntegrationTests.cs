using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StarTrekWeather.Data;
using StarTrekWeather.Models;
using Xunit;
 
namespace StarTrekWeatherTests
{
    // ---------------------------------------------------------------------------
    // Custom WebApplicationFactory that swaps Postgres for SQLite in-memory
    // ---------------------------------------------------------------------------
    public class TestWebAppFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the real Postgres DbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);
 
                // Add SQLite in-memory database
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite("DataSource=:memory:"));
 
                // Ensure schema is created and seed a test user
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.OpenConnection();
                db.Database.EnsureCreated();
 
                db.Users.Add(new User
                {
                    Username = "testuser",
                    PasswordHash = "hashed"
                });
                db.SaveChanges();
            });
        }
    }
 
    // ---------------------------------------------------------------------------
    // Helper to deserialize responses cleanly
    // ---------------------------------------------------------------------------
    file static class ResponseHelper
    {
        private static readonly JsonSerializerOptions Opts = new() { PropertyNameCaseInsensitive = true };
 
        public static async Task<T?> ReadAsync<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, Opts);
        }
    }
 
    // ---------------------------------------------------------------------------
    // User Pokemon Location tests
    // ---------------------------------------------------------------------------
    public class UserPokemonLocationTests : IClassFixture<TestWebAppFactory>
    {
        private readonly HttpClient _client;
 
        public UserPokemonLocationTests(TestWebAppFactory factory)
        {
            _client = factory.CreateClient();
 
            // Simulate a logged-in session for Razor page handlers
            _client.DefaultRequestHeaders.Add("Cookie", "");
        }
 
        [Fact]
        public async Task PostLocation_ValidUser_Returns200()
        {
            var response = await _client.PostAsJsonAsync("/pokemonlocations/users", new
            {
                Username = "testuser",
                LocationName = "Pallet Town"
            });
 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
 
        [Fact]
        public async Task PostLocation_UnknownUser_Returns404()
        {
            var response = await _client.PostAsJsonAsync("/pokemonlocations/users", new
            {
                Username = "nobody",
                LocationName = "Pallet Town"
            });
 
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
 
        [Fact]
        public async Task PostLocation_Duplicate_Returns409()
        {
            await _client.PostAsJsonAsync("/pokemonlocations/users", new
            {
                Username = "testuser",
                LocationName = "Cerulean City"
            });
 
            var response = await _client.PostAsJsonAsync("/pokemonlocations/users", new
            {
                Username = "testuser",
                LocationName = "Cerulean City"
            });
 
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }
 
        [Fact]
        public async Task GetLocations_ReturnsOnlySavedLocations()
        {
            await _client.PostAsJsonAsync("/pokemonlocations/users", new
            {
                Username = "testuser",
                LocationName = "Vermilion City"
            });
 
            var response = await _client.GetAsync("/pokemonlocations/users/testuser");
 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var locations = await ResponseHelper.ReadAsync<List<string>>(response);
            Assert.NotNull(locations);
            Assert.Contains("Vermilion City", locations);
        }
 
        [Fact]
        public async Task GetLocations_UnknownUser_Returns404()
        {
            var response = await _client.GetAsync("/pokemonlocations/users/nobody");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
 
        [Fact]
        public async Task DeleteLocation_RemovesSuccessfully()
        {
            await _client.PostAsJsonAsync("/pokemonlocations/users", new
            {
                Username = "testuser",
                LocationName = "Lavender Town"
            });
 
            var deleteResponse = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "/pokemonlocations/users")
            {
                Content = JsonContent.Create(new { Username = "testuser", LocationName = "Lavender Town" })
            });
 
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
 
            var getResponse = await _client.GetAsync("/pokemonlocations/users/testuser");
            var locations = await ResponseHelper.ReadAsync<List<string>>(getResponse);
            Assert.DoesNotContain("Lavender Town", locations ?? []);
        }
 
        [Fact]
        public async Task DeleteLocation_NotFound_Returns404()
        {
            var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "/pokemonlocations/users")
            {
                Content = JsonContent.Create(new { Username = "testuser", LocationName = "Nonexistent Place" })
            });
 
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
 
        [Fact]
        public async Task GetTemp_SavedLocation_ReturnsTemperatureInRange()
        {
            await _client.PostAsJsonAsync("/pokemonlocations/users", new
            {
                Username = "testuser",
                LocationName = "Fuchsia City"
            });
 
            var response = await _client.GetAsync("/pokemonlocations/users/testuser/Fuchsia City/temp");
 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await ResponseHelper.ReadAsync<TempResult>(response);
            Assert.NotNull(result);
            Assert.InRange(result.Temperature, -20.0, 45.0);
        }
 
        [Fact]
        public async Task GetTemp_UnsavedLocation_Returns404()
        {
            var response = await _client.GetAsync("/pokemonlocations/users/testuser/Nowhere/temp");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
 
        private record TempResult(string Username, string LocationName, double Temperature);
    }
 
    // ---------------------------------------------------------------------------
    // User Pokemon Location Gym tests
    // ---------------------------------------------------------------------------
    public class UserPokemonLocationGymTests : IClassFixture<TestWebAppFactory>
    {
        private readonly HttpClient _client;
 
        public UserPokemonLocationGymTests(TestWebAppFactory factory)
        {
            _client = factory.CreateClient();
        }
 
        private async Task EnsureLocationExists(string locationName)
        {
            await _client.PostAsJsonAsync("/pokemonlocations/users", new
            {
                Username = "testuser",
                LocationName = locationName
            });
        }
 
        [Fact]
        public async Task PostGym_ValidLocation_Returns200()
        {
            await EnsureLocationExists("Pewter City");
 
            var response = await _client.PostAsJsonAsync(
                "/pokemonlocations/users/testuser/Pewter City/gyms",
                new { GymName = "Brock's Gym" });
 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
 
        [Fact]
        public async Task PostGym_LocationNotFound_Returns404()
        {
            var response = await _client.PostAsJsonAsync(
                "/pokemonlocations/users/testuser/Nonexistent/gyms",
                new { GymName = "Ghost Gym" });
 
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
 
        [Fact]
        public async Task PostGym_Duplicate_Returns409()
        {
            await EnsureLocationExists("Cerulean City");
            await _client.PostAsJsonAsync(
                "/pokemonlocations/users/testuser/Cerulean City/gyms",
                new { GymName = "Misty's Gym" });
 
            var response = await _client.PostAsJsonAsync(
                "/pokemonlocations/users/testuser/Cerulean City/gyms",
                new { GymName = "Misty's Gym" });
 
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }
 
        [Fact]
        public async Task GetGyms_ReturnsSavedGyms()
        {
            await EnsureLocationExists("Saffron City");
            await _client.PostAsJsonAsync(
                "/pokemonlocations/users/testuser/Saffron City/gyms",
                new { GymName = "Sabrina's Gym" });
 
            var response = await _client.GetAsync("/pokemonlocations/users/testuser/Saffron City/gyms");
 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var gyms = await ResponseHelper.ReadAsync<List<string>>(response);
            Assert.NotNull(gyms);
            Assert.Contains("Sabrina's Gym", gyms);
        }
 
        [Fact]
        public async Task DeleteGym_RemovesSuccessfully()
        {
            await EnsureLocationExists("Viridian City");
            await _client.PostAsJsonAsync(
                "/pokemonlocations/users/testuser/Viridian City/gyms",
                new { GymName = "Giovanni's Gym" });
 
            var response = await _client.DeleteAsync(
                "/pokemonlocations/users/testuser/Viridian City/gyms/Giovanni's Gym");
 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
 
            var gyms = await ResponseHelper.ReadAsync<List<string>>(
                await _client.GetAsync("/pokemonlocations/users/testuser/Viridian City/gyms"));
            Assert.DoesNotContain("Giovanni's Gym", gyms ?? []);
        }
 
        [Fact]
        public async Task DeleteGym_NotFound_Returns404()
        {
            await EnsureLocationExists("Cinnabar Island");
 
            var response = await _client.DeleteAsync(
                "/pokemonlocations/users/testuser/Cinnabar Island/gyms/Blaine's Gym");
 
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
 
    // ---------------------------------------------------------------------------
    // User Pokemon Location Building tests
    // ---------------------------------------------------------------------------
    public class UserPokemonLocationBuildingTests : IClassFixture<TestWebAppFactory>
    {
        private readonly HttpClient _client;
 
        public UserPokemonLocationBuildingTests(TestWebAppFactory factory)
        {
            _client = factory.CreateClient();
        }
 
        private async Task EnsureLocationExists(string locationName)
        {
            await _client.PostAsJsonAsync("/pokemonlocations/users", new
            {
                Username = "testuser",
                LocationName = locationName
            });
        }
 
        [Fact]
        public async Task PostBuilding_ValidLocation_Returns200()
        {
            await EnsureLocationExists("Pallet Town");
 
            var response = await _client.PostAsJsonAsync(
                "/pokemonlocations/users/testuser/Pallet Town/buildings",
                new { BuildingName = "Oak's Lab" });
 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
 
        [Fact]
        public async Task PostBuilding_LocationNotFound_Returns404()
        {
            var response = await _client.PostAsJsonAsync(
                "/pokemonlocations/users/testuser/Nonexistent/buildings",
                new { BuildingName = "Mystery Building" });
 
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
 
        [Fact]
        public async Task PostBuilding_Duplicate_Returns409()
        {
            await EnsureLocationExists("Goldenrod City");
            await _client.PostAsJsonAsync(
                "/pokemonlocations/users/testuser/Goldenrod City/buildings",
                new { BuildingName = "Pokemon Center" });
 
            var response = await _client.PostAsJsonAsync(
                "/pokemonlocations/users/testuser/Goldenrod City/buildings",
                new { BuildingName = "Pokemon Center" });
 
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }
 
        [Fact]
        public async Task GetBuildings_ReturnsSavedBuildings()
        {
            await EnsureLocationExists("Ecruteak City");
            await _client.PostAsJsonAsync(
                "/pokemonlocations/users/testuser/Ecruteak City/buildings",
                new { BuildingName = "Tin Tower" });
 
            var response = await _client.GetAsync(
                "/pokemonlocations/users/testuser/Ecruteak City/buildings");
 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var buildings = await ResponseHelper.ReadAsync<List<string>>(response);
            Assert.NotNull(buildings);
            Assert.Contains("Tin Tower", buildings);
        }
 
        [Fact]
        public async Task DeleteBuilding_RemovesSuccessfully()
        {
            await EnsureLocationExists("Mahogany Town");
            await _client.PostAsJsonAsync(
                "/pokemonlocations/users/testuser/Mahogany Town/buildings",
                new { BuildingName = "Rocket HQ" });
 
            var response = await _client.DeleteAsync(
                "/pokemonlocations/users/testuser/Mahogany Town/buildings/Rocket HQ");
 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
 
            var buildings = await ResponseHelper.ReadAsync<List<string>>(
                await _client.GetAsync("/pokemonlocations/users/testuser/Mahogany Town/buildings"));
            Assert.DoesNotContain("Rocket HQ", buildings ?? []);
        }
 
        [Fact]
        public async Task DeleteBuilding_NotFound_Returns404()
        {
            await EnsureLocationExists("Blackthorn City");
 
            var response = await _client.DeleteAsync(
                "/pokemonlocations/users/testuser/Blackthorn City/buildings/Dragon's Den");
 
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
 
        [Fact]
        public async Task GetBuildings_LocationWithMultipleBuildings_ReturnsAll()
        {
            await EnsureLocationExists("Goldenrod City");
            await _client.PostAsJsonAsync(
                "/pokemonlocations/users/testuser/Goldenrod City/buildings",
                new { BuildingName = "Radio Tower" });
            await _client.PostAsJsonAsync(
                "/pokemonlocations/users/testuser/Goldenrod City/buildings",
                new { BuildingName = "Department Store" });
            await _client.PostAsJsonAsync(
                "/pokemonlocations/users/testuser/Goldenrod City/buildings",
                new { BuildingName = "Game Corner" });
 
            var response = await _client.GetAsync(
                "/pokemonlocations/users/testuser/Goldenrod City/buildings");
 
            var buildings = await ResponseHelper.ReadAsync<List<string>>(response);
            Assert.NotNull(buildings);
            Assert.Contains("Radio Tower", buildings);
            Assert.Contains("Department Store", buildings);
            Assert.Contains("Game Corner", buildings);
        }
    }
}