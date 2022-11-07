using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Account> Accounts { get; }
    public DbSet<RefreshToken> RefreshTokens { get; }
    public DbSet<Student> Students { get; }
    public DbSet<Company> Companies { get; }
    public DbSet<Admin> Admins { get; }
    public DbSet<JobOffer> JobOffers { get; }
    public DbSet<Tag> Tags { get; }
    public DbSet<StudentSubscription> StudentSubscriptions { get; }
    public DbSet<StudentGroup> StudentGroups { get; }
    public DbSet<JobPosition> JobPositions { get; }
    public DbSet<Experience> Experiences { get; }
    public DbSet<ForeignLanguage> ForeignLanguages { get; }
    public DbSet<CVProjectLink> CVProjectLinks { get; }
    public DbSet<Education> Educations { get; }
    public DbSet<CV> CVs { get; }
    public DbSet<StudentLog> StudentLogs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
