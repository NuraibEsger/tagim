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
        modelBuilder.Entity<BaseEntity>().HasQueryFilter(b => !b.IsDeleted);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        ChangeTracker.DetectChanges();
        foreach (var item in ChangeTracker
                     .Entries<BaseEntity>()
                     .Where(e => e.State == EntityState.Deleted))
        {
            item.State = EntityState.Modified;
            item.Entity.IsDeleted = true;
        }
        
        return await base.SaveChangesAsync(cancellationToken);
    }
}