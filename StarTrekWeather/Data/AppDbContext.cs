using Microsoft.EntityFrameworkCore;
using StarTrekWeather.Models;
 
namespace  StarTrekWeather.Data {
 
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<UserPlanet> UserPlanets => Set<UserPlanet>();
        public DbSet<UserPokemonLocation> UserPokemonLocations => Set<UserPokemonLocation>();
        public DbSet<UserPokemonLocationGym> UserPokemonLocationGyms => Set<UserPokemonLocationGym>();
        public DbSet<UserPokemonLocationBuilding> UserPokemonLocationBuildings => Set<UserPokemonLocationBuilding>();
 
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // USERS
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
 
                entity.HasKey(u => u.Username);
 
                entity.Property(u => u.Username)
                    .HasColumnName("username")
                    .HasMaxLength(20);
 
                entity.Property(u => u.PasswordHash)
                    .HasColumnName("password_hash")
                    .HasMaxLength(255);
            });
 
            // USER_PLANETS
            modelBuilder.Entity<UserPlanet>(entity =>
            {
                entity.ToTable("user_planets");
 
                entity.HasKey(up => new { up.Username, up.PlanetName });
 
                entity.Property(up => up.Username)
                    .HasColumnName("users_username");
 
                entity.Property(up => up.PlanetName)
                    .HasColumnName("planets_name");
 
                entity.HasOne(up => up.User)
                    .WithMany(u => u.UserPlanets)
                    .HasForeignKey(up => up.Username)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            // USER_POKEMON_LOCATIONS
            modelBuilder.Entity<UserPokemonLocation>(entity =>
            {
                entity.ToTable("user_pokemon_locations");
 
                entity.HasKey(ul => new { ul.Username, ul.LocationName });
 
                entity.Property(ul => ul.Username)
                    .HasColumnName("users_username");
 
                entity.Property(ul => ul.LocationName)
                    .HasColumnName("location_name")
                    .HasMaxLength(100);
 
                entity.HasOne(ul => ul.User)
                    .WithMany(u => u.UserPokemonLocations)
                    .HasForeignKey(ul => ul.Username)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            // USER_POKEMON_LOCATION_GYMS
            modelBuilder.Entity<UserPokemonLocationGym>(entity =>
            {
                entity.ToTable("user_pokemon_location_gyms");
 
                entity.HasKey(g => g.Id);
 
                entity.Property(g => g.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();
 
                entity.Property(g => g.Username)
                    .HasColumnName("users_username");
 
                entity.Property(g => g.LocationName)
                    .HasColumnName("location_name")
                    .HasMaxLength(100);
 
                entity.Property(g => g.GymName)
                    .HasColumnName("gym_name")
                    .HasMaxLength(100);
 
                entity.HasOne(g => g.UserPokemonLocation)
                    .WithMany()
                    .HasForeignKey(g => new { g.Username, g.LocationName })
                    .OnDelete(DeleteBehavior.Cascade);
            });
            // USER_POKEMON_LOCATION_BUILDINGS
            modelBuilder.Entity<UserPokemonLocationBuilding>(entity =>
            {
                entity.ToTable("user_pokemon_location_buildings");
 
                entity.HasKey(b => b.Id);
 
                entity.Property(b => b.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();
 
                entity.Property(b => b.Username)
                    .HasColumnName("users_username");
 
                entity.Property(b => b.LocationName)
                    .HasColumnName("location_name")
                    .HasMaxLength(100);
 
                entity.Property(b => b.BuildingName)
                    .HasColumnName("building_name")
                    .HasMaxLength(100);
 
                entity.HasOne(b => b.UserPokemonLocation)
                    .WithMany()
                    .HasForeignKey(b => new { b.Username, b.LocationName })
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}