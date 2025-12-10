using Microsoft.EntityFrameworkCore;
using Tagim.Domain.Common;

namespace Tagim.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Vehicle> Vehicles { get; set; }
    DbSet<Tag> Tags { get; set; }
    DbSet<SocialMediaLink> SocialMediaLinks { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}