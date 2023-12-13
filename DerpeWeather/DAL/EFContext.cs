using DerpeWeather.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DerpeWeather.DAL
{
    /// <summary>
    /// Custom Entity Framework context for this app.
    /// </summary>
    public class EFContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserAppPreferencesEntity> UserAppPreferences { get; set; }
        public DbSet<UserTrackedWeatherFieldsEntity> UserTrackedWeatherFields { get; set; }



        public EFContext() : base() { }

        public EFContext(DbContextOptions<EFContext> options) : base(options) { }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(App.SQLConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .HasOne(u => u.AppPreferences)
                .WithOne(ap => ap.User)
                .HasForeignKey<UserAppPreferencesEntity>(ap => ap.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<UserEntity>()
                .HasMany(u => u.TrackedWeatherFields)
                .WithOne(twf => twf.User)
                .HasForeignKey(twf => twf.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
