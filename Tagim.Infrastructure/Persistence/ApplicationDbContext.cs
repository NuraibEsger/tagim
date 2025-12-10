using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Tagim.Application.Interfaces;
using Tagim.Domain.Common;

namespace Tagim.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : 
    DbContext(options), IApplicationDbContext
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<SocialMediaLink> SocialMediaLinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}