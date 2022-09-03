﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Account> Accounts { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Student> Students { get; }
    DbSet<Company> Companies { get; }
    DbSet<Admin> Admins { get; }
    DbSet<Image> Images { get; }
    DbSet<StudentGroup> StudentGroups { get; }
    DbSet<StudentLog> StudentLogs { get; }
    DbSet<CompanyLink> CompanyLinks { get; }
    DbSet<StudentSubscription> StudentSubscriptions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
