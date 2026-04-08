
using Microsoft.EntityFrameworkCore;
using StarTrekWeather.Models;

namespace  StarTrekWeather.Data {

    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<UserPlanet> UserPlanets => Set<UserPlanet>();

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
        }
    }
}