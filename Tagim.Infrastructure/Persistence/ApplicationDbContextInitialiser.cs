using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tagim.Domain.Common;
using Tagim.Domain.Enums;

namespace Tagim.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser(
    ApplicationDbContext context,
    ILogger<ApplicationDbContextInitialiser> logger)
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<ApplicationDbContextInitialiser> _logger = logger;

    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.IsNpgsql())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Verilənlər bazası başladılarkən xəta baş verdi.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Məlumatlar doldurularkən (Seeding) xəta baş verdi.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (!await _context.Users.AnyAsync())
        {
            var admin = new User
            {
                FullName = "Admin",
                Email = "admin@tagim.az",
                PhoneNumber = "0000000000",
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            };
            
            await _context.Users.AddAsync(admin);
            await _context.SaveChangesAsync();
        }
    }
}