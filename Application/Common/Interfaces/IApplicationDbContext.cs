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
    public DbSet<CV> CVs { get; }
    public DbSet<StudentLog> StudentLogs { get; }
    public DbSet<Post> Posts { get; }
    public DbSet<Ban> Bans { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
