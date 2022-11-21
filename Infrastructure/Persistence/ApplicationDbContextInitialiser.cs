using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

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
                await _context.Database.EnsureDeletedAsync();
                await _context.Database.MigrateAsync();

                _context.Database.OpenConnection();
                ((NpgsqlConnection)_context.Database.GetDbConnection()).ReloadTypes();
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
        #region SeedAdmins

        if (!await _context.Admins.AnyAsync())
        {
            await _context.Admins.AddAsync(new Admin
            {
                Email = "Admin@CareerHub.ua",
                PasswordHash = "$2a$11$aQ3eaj6dZVNkWqaRFtJLy.7Jt0.Xx0ebv6UHOQSUd1jLEhy4hZZka",
                Verified = DateTime.UtcNow,
                IsSuperAdmin = true
            });
        }

        #endregion

        #region SeedStudentGroups

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
                    Name = "ПЗПІ-19-9"
                },
                new StudentGroup
                {
                    Name = "ПЗПІ-19-10"
                },
                new StudentGroup
                {
                    Name = "ПЗПІ-19-11"
                },
                new StudentGroup
                {
                    Name = "ПЗПІи-19-1"
                }
            );
        }

        #endregion

        #region SeedJobPositions

        if (!await _context.JobPositions.AnyAsync())
        {
            await _context.JobPositions.AddRangeAsync(
                new JobPosition()
                {
                    Name = "Software development"
                },
                new JobPosition()
                {
                    Name = "Quality assurance"
                },
                new JobPosition()
                {
                    Name = "Design"
                },
                new JobPosition()
                {
                    Name = "DevOps"
                },
                new JobPosition()
                {
                    Name = "Support"
                },
                new JobPosition()
                {
                    Name = "HR"
                },
                new JobPosition()
                {
                    Name = "Finances"
                },
                new JobPosition()
                {
                    Name = "Other"
                }
            );
        }

        #endregion

        #region SeedTags

        if (!await _context.Tags.AnyAsync())
        {
            await _context.Tags.AddRangeAsync(
                new Tag()
                {
                    Name = "C#",
                    IsAccepted = true
                },
                new Tag()
                {
                    Name = "F#"
                },
                new Tag()
                {
                    Name = "OOP",
                    IsAccepted = true
                },
                new Tag()
                {
                    Name = "OOD"
                },
                new Tag()
                {
                    Name = "ASP.NET Core",
                    IsAccepted = true
                },
                new Tag()
                {
                    Name = ".NET"
                },
                new Tag()
                {
                    Name = "EF Core",
                    IsAccepted = true
                },
                new Tag()
                {
                    Name = "MediatR"
                },
                new Tag()
                {
                    Name = "RabbitMQ",
                    IsAccepted = true
                },
                new Tag()
                {
                    Name = "xUnit"
                }
            );
        }

        #endregion

        await _context.SaveChangesAsync();

        #region SeedStudentLogs

        if (!await _context.StudentLogs.AnyAsync() && await _context.StudentGroups.AnyAsync() && !await _context.Students.AnyAsync())
        {
            var studentLog = await _context.StudentGroups.ToListAsync();
            await _context.StudentLogs.AddRangeAsync(
                new StudentLog()
                {
                    FirstName = "Микола",
                    LastName = "Берковський",
                    Email = "mykola.berkovskyi@nure.ua",
                    StudentGroup = studentLog.OrderBy(x => Guid.NewGuid()).First()
                },
                new StudentLog()
                {
                    FirstName = "Юлія",
                    LastName = "Коба",
                    Email = "yuliia.koba@nure.ua",
                    StudentGroup = studentLog.OrderBy(x => Guid.NewGuid()).First()
                },
                new StudentLog()
                {
                    FirstName = "Сергій",
                    LastName = "Бурцев",
                    Email = "serhii.burtsev@nure.ua",
                    StudentGroup = studentLog.OrderBy(x => Guid.NewGuid()).First()
                }
            );
        }

        #endregion

        #region SeedCompanies

        if (!await _context.Companies.AnyAsync())
        {
            await _context.Companies.AddRangeAsync(
                new Company()
                {
                    Name = "Company1",
                    Motto = "Motto1",
                    Description = "Description1",
                    Email = "Company1@company.com",
                    PasswordHash = "$2a$11$aQ3eaj6dZVNkWqaRFtJLy.7Jt0.Xx0ebv6UHOQSUd1jLEhy4hZZka",
                    Verified = DateTime.UtcNow,
                    Links = new List<CompanyLink> { new CompanyLink { Title = "Title", Uri = "Uri" } }
                },
                new Company()
                {
                    Name = "Company2",
                    Motto = "Motto2",
                    Description = "Description2",
                    Email = "Company2@company.com",
                    PasswordHash = "$2a$11$aQ3eaj6dZVNkWqaRFtJLy.7Jt0.Xx0ebv6UHOQSUd1jLEhy4hZZka",
                    Verified = DateTime.UtcNow,
                    Links = new List<CompanyLink> { new CompanyLink { Title = "Title", Uri = "Uri" } }
                },
                new Company()
                {
                    Name = "Company3",
                    Motto = "Motto3",
                    Description = "Description3",
                    Email = "Company3@company.com",
                    PasswordHash = "$2a$11$aQ3eaj6dZVNkWqaRFtJLy.7Jt0.Xx0ebv6UHOQSUd1jLEhy4hZZka",
                    Verified = DateTime.UtcNow,
                    Links = new List<CompanyLink> { new CompanyLink { Title = "Title", Uri = "Uri" } }
                }
            );
        }

        #endregion

        await _context.SaveChangesAsync();
    }
}
