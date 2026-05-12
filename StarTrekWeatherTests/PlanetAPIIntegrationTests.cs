using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
 
namespace StarTrekWeatherAPITests
{
    // ---------------------------------------------------------------------------
    // Custom WebApplicationFactory — swaps Postgres for SQLite in-memory
    // and seeds a known set of planets
    // ---------------------------------------------------------------------------
    public class ApiTestWebAppFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the real Postgres DbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApiDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);
 
                // Add SQLite in-memory database
                services.AddDbContext<ApiDbContext>(options =>
                    options.UseSqlite("DataSource=:memory:"));
 
                // Create schema and seed test planets
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
                db.Database.OpenConnection();
                db.Database.EnsureCreated();
 
                db.Planets.AddRange(
                    new Planet
                    {
                        Name = "Vulcan",
                        SolarSystem = "Vulcan System",
                        MaxTemp = 67.0f,
                        MinTemp = 18.0f,
                        AtmosphericPressure = 0.5f,
                        Description = "Arid desert world, home of the Vulcans.",
                        ImageUrl = "/images/planets/vulcan.jpg"
                    },
                    new Planet
                    {
                        Name = "Risa",
                        SolarSystem = "Risian System",
                        MaxTemp = 35.0f,
                        MinTemp = 22.0f,
                        AtmosphericPressure = 1.0f,
                        Description = "Pleasure planet with a perfectly controlled climate.",
                        ImageUrl = "/images/planets/risa.jpg"
                    },
                    new Planet
                    {
                        Name = "Andoria",
                        SolarSystem = "Andorian System",
                        MaxTemp = -15.0f,
                        MinTemp = -80.0f,
                        AtmosphericPressure = 0.9f,
                        Description = "Frozen moon, home of the Andorians.",
                        ImageUrl = "/images/planets/andoria.jpg"
                    }
                );
                db.SaveChanges();
            });
        }
    }
 
    // ---------------------------------------------------------------------------
    // Helper — builds a valid Basic Auth header
    // ---------------------------------------------------------------------------
    file static class Auth
    {
        public static AuthenticationHeaderValue ValidHeader()
        {
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:password123"));
            return new AuthenticationHeaderValue("Basic", credentials);
        }
 
        public static AuthenticationHeaderValue InvalidHeader()
        {
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes("wrong:credentials"));
            return new AuthenticationHeaderValue("Basic", credentials);
        }
    }
 
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
    // Basic Auth middleware tests
    // ---------------------------------------------------------------------------
    public class BasicAuthTests : IClassFixture<ApiTestWebAppFactory>
    {
        private readonly HttpClient _client;
 
        public BasicAuthTests(ApiTestWebAppFactory factory)
        {
            _client = factory.CreateClient();
        }
 
        [Fact]
        public async Task Request_WithNoAuth_Returns401()
        {
            var response = await _client.GetAsync("/api/planet");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
 
        [Fact]
        public async Task Request_WithInvalidCredentials_Returns401()
        {
            _client.DefaultRequestHeaders.Authorization = Auth.InvalidHeader();
            var response = await _client.GetAsync("/api/planet");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
 
        [Fact]
        public async Task Request_WithValidCredentials_Returns200()
        {
            _client.DefaultRequestHeaders.Authorization = Auth.ValidHeader();
            var response = await _client.GetAsync("/api/planet");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
 
        [Fact]
        public async Task Unauthorized_Response_IncludesWwwAuthenticateHeader()
        {
            var response = await _client.GetAsync("/api/planet");
            Assert.True(response.Headers.Contains("WWW-Authenticate"));
        }
    }
 
    // ---------------------------------------------------------------------------
    // GET /api/planet — get all planets
    // ---------------------------------------------------------------------------
    public class GetAllPlanetsTests : IClassFixture<ApiTestWebAppFactory>
    {
        private readonly HttpClient _client;
 
        public GetAllPlanetsTests(ApiTestWebAppFactory factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = Auth.ValidHeader();
        }
 
        [Fact]
        public async Task GetAll_Returns200()
        {
            var response = await _client.GetAsync("/api/planet");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
 
        [Fact]
        public async Task GetAll_ReturnsAllSeededPlanets()
        {
            var response = await _client.GetAsync("/api/planet");
            var planets = await ResponseHelper.ReadAsync<List<Planet>>(response);
 
            Assert.NotNull(planets);
            Assert.Equal(3, planets.Count);
        }
 
        [Fact]
        public async Task GetAll_PlanetsHaveExpectedFields()
        {
            var response = await _client.GetAsync("/api/planet");
            var planets = await ResponseHelper.ReadAsync<List<Planet>>(response);
 
            Assert.NotNull(planets);
            var vulcan = planets.FirstOrDefault(p => p.Name == "Vulcan");
            Assert.NotNull(vulcan);
            Assert.Equal("Vulcan System", vulcan.SolarSystem);
            Assert.Equal(67.0f, vulcan.MaxTemp);
            Assert.Equal(18.0f, vulcan.MinTemp);
            Assert.Equal(0.5f, vulcan.AtmosphericPressure);
        }
 
        [Fact]
        public async Task GetAll_ImageUrls_AreAbsolute()
        {
            var response = await _client.GetAsync("/api/planet");
            var planets = await ResponseHelper.ReadAsync<List<Planet>>(response);
 
            Assert.NotNull(planets);
            Assert.All(planets, p =>
                Assert.True(
                    p.ImageUrl.StartsWith("http://") || p.ImageUrl.StartsWith("https://"),
                    $"ImageUrl '{p.ImageUrl}' is not absolute"
                )
            );
        }
    }
 
    // ---------------------------------------------------------------------------
    // GET /api/planet/{name} — get planet by name
    // ---------------------------------------------------------------------------
    public class GetPlanetByNameTests : IClassFixture<ApiTestWebAppFactory>
    {
        private readonly HttpClient _client;
 
        public GetPlanetByNameTests(ApiTestWebAppFactory factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = Auth.ValidHeader();
        }
 
        [Fact]
        public async Task GetByName_ExistingPlanet_Returns200()
        {
            var response = await _client.GetAsync("/api/planet/Vulcan");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
 
        [Fact]
        public async Task GetByName_ReturnsCorrectPlanet()
        {
            var response = await _client.GetAsync("/api/planet/Risa");
            var planet = await ResponseHelper.ReadAsync<Planet>(response);
 
            Assert.NotNull(planet);
            Assert.Equal("Risa", planet.Name);
            Assert.Equal("Risian System", planet.SolarSystem);
            Assert.Equal(35.0f, planet.MaxTemp);
            Assert.Equal(22.0f, planet.MinTemp);
        }
 
        [Fact]
        public async Task GetByName_IsCaseInsensitive()
        {
            var response = await _client.GetAsync("/api/planet/vulcan");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
 
            var planet = await ResponseHelper.ReadAsync<Planet>(response);
            Assert.NotNull(planet);
            Assert.Equal("Vulcan", planet.Name);
        }
 
        [Fact]
        public async Task GetByName_NonExistentPlanet_Returns404()
        {
            var response = await _client.GetAsync("/api/planet/Kronos");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
 
        [Fact]
        public async Task GetByName_ImageUrl_IsAbsolute()
        {
            var response = await _client.GetAsync("/api/planet/Andoria");
            var planet = await ResponseHelper.ReadAsync<Planet>(response);
 
            Assert.NotNull(planet);
            Assert.True(
                planet.ImageUrl.StartsWith("http://") || planet.ImageUrl.StartsWith("https://"),
                $"ImageUrl '{planet.ImageUrl}' is not absolute"
            );
        }
    }
 
    // ---------------------------------------------------------------------------
    // GET /api/planet/search?q= — search planets by name
    // ---------------------------------------------------------------------------
    public class SearchPlanetsTests : IClassFixture<ApiTestWebAppFactory>
    {
        private readonly HttpClient _client;
 
        public SearchPlanetsTests(ApiTestWebAppFactory factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = Auth.ValidHeader();
        }
 
        [Fact]
        public async Task Search_MatchingQuery_ReturnsResults()
        {
            var response = await _client.GetAsync("/api/planet/search?q=ul");
            var planets = await ResponseHelper.ReadAsync<List<Planet>>(response);
 
            Assert.NotNull(planets);
            Assert.Contains(planets, p => p.Name == "Vulcan");
        }
 
        [Fact]
        public async Task Search_IsCaseInsensitive()
        {
            var response = await _client.GetAsync("/api/planet/search?q=RISA");
            var planets = await ResponseHelper.ReadAsync<List<Planet>>(response);
 
            Assert.NotNull(planets);
            Assert.Contains(planets, p => p.Name == "Risa");
        }
 
        [Fact]
        public async Task Search_NoMatch_ReturnsEmptyList()
        {
            var response = await _client.GetAsync("/api/planet/search?q=Kronos");
            var planets = await ResponseHelper.ReadAsync<List<Planet>>(response);
 
            Assert.NotNull(planets);
            Assert.Empty(planets);
        }
 
        [Fact]
        public async Task Search_EmptyQuery_ReturnsEmptyList()
        {
            var response = await _client.GetAsync("/api/planet/search?q=");
            var planets = await ResponseHelper.ReadAsync<List<Planet>>(response);
 
            Assert.NotNull(planets);
            Assert.Empty(planets);
        }
 
        [Fact]
        public async Task Search_ReturnsAtMostFiveResults()
        {
            // "a" matches Vulcan, Risa, Andoria — all three in our seed
            var response = await _client.GetAsync("/api/planet/search?q=a");
            var planets = await ResponseHelper.ReadAsync<List<Planet>>(response);
 
            Assert.NotNull(planets);
            Assert.True(planets.Count <= 5);
        }
    }
}