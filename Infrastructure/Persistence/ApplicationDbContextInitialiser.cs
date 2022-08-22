using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
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
            _logger.LogError(ex, "An error occurred while initialising the database.");
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
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default admin accounts
        if (!await _context.Admins.AnyAsync())
        {
            await _context.Admins.AddAsync(new Admin
            {
                Email = "Admin@CareerHub.ua",
                PasswordHash = "$2a$11$aQ3eaj6dZVNkWqaRFtJLy.7Jt0.Xx0ebv6UHOQSUd1jLEhy4hZZka",
                Verified = DateTime.UtcNow
            });
        }

        // Default StudentGroups
        if (!await _context.StudentGroups.AnyAsync())
        {
            await _context.StudentGroups.AddRangeAsync(
                new StudentGroup
                {
                    Name = "ПЗПІ-19-6"
                },
                new StudentGroup
                {
                    Name = "ПЗПІ-19-7"
                },
                new StudentGroup
                {
                    Name = "ПЗПІ-19-8"
                },
                new StudentGroup
                {
                    Name = "ПЗПІи-19-1"
                }
            );
        }

        await _context.SaveChangesAsync();
    }
}
