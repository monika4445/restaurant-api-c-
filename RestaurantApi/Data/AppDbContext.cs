using Microsoft.EntityFrameworkCore;
using RestaurantApi.Domain;

namespace RestaurantApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<Favorite> Favorites => Set<Favorite>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Restaurant>(b =>
        {
            b.HasKey(r => r.Id);
            b.Property(r => r.Name).IsRequired().HasMaxLength(200);
            b.Property(r => r.Address).IsRequired().HasMaxLength(500);
            b.Property(r => r.ContactNumber).IsRequired().HasMaxLength(50);
            b.Property(r => r.HoursOfOperation).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<Player>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            b.Property(p => p.LastName).IsRequired().HasMaxLength(100);
            b.Property(p => p.MobileNumber).IsRequired().HasMaxLength(50);
            b.Property(p => p.Email).IsRequired().HasMaxLength(200);
            b.Property(p => p.DriversLicense).IsRequired().HasMaxLength(50);
            b.Property(p => p.Passport).IsRequired().HasMaxLength(50);


            b.OwnsOne(p => p.PrimaryAddress, ConfigureAddress);
            b.OwnsOne(p => p.AlternateAddress, ConfigureAddress);
            b.OwnsOne(p => p.OfficeAddress, ConfigureAddress);
        });

        modelBuilder.Entity<Membership>(b =>
        {
            b.HasKey(m => m.Id);
            b.HasIndex(m => new { m.PlayerId, m.RestaurantId }).IsUnique();
            b.HasOne(m => m.Player)
                .WithMany(p => p.Memberships)
                .HasForeignKey(m => m.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(m => m.Restaurant)
                .WithMany(r => r.Memberships)
                .HasForeignKey(m => m.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Favorite>(b =>
        {
            b.HasKey(f => f.Id);
            b.HasIndex(f => new { f.PlayerId, f.RestaurantId }).IsUnique();
            b.HasOne(f => f.Player)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(f => f.Restaurant)
                .WithMany(r => r.Favorites)
                .HasForeignKey(f => f.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureAddress<TOwner>(
        Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder<TOwner, Address> a)
        where TOwner : class
    {
        a.Property(x => x.StreetNumber).HasMaxLength(50);
        a.Property(x => x.Line1).HasMaxLength(200);
        a.Property(x => x.Line2).HasMaxLength(200);
        a.Property(x => x.City).HasMaxLength(100);
        a.Property(x => x.State).HasMaxLength(100);
        a.Property(x => x.Postal).HasMaxLength(20);
        a.Property(x => x.Country).HasMaxLength(100);
    }
}
