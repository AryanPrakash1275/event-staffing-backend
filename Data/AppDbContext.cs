using EventStaffing.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EventStaffing.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Volunteer> Volunteers => Set<Volunteer>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Application> Applications => Set<Application>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Volunteer>()
            .HasIndex(v => v.Phone)
            .IsUnique();

        modelBuilder.Entity<Application>()
            .HasIndex(a => new { a.EventId, a.VolunteerId })
            .IsUnique();

        modelBuilder.Entity<Application>()
            .HasOne(a => a.Event)
            .WithMany(e => e.Applications)
            .HasForeignKey(a => a.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Application>()
            .HasOne(a => a.Volunteer)
            .WithMany(v => v.Applications)
            .HasForeignKey(a => a.VolunteerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}