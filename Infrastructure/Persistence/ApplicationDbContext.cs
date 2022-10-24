using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Common;
using Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    private readonly ILoggerFactory _loggerFactory;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        _loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<CompanyLink> CompanyLinks => Set<CompanyLink>();
    public DbSet<JobOffer> JobOffers => Set<JobOffer>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<StudentSubscription> StudentSubscriptions => Set<StudentSubscription>();
    public DbSet<StudentGroup> StudentGroups => Set<StudentGroup>();
    public DbSet<JobPosition> JobPositions => Set<JobPosition>();
    public DbSet<Experience> Experiences => Set<Experience>();
    public DbSet<ForeignLanguage> ForeignLanguages => Set<ForeignLanguage>();
    public DbSet<CVProjectLink> CVProjectLinks => Set<CVProjectLink>();
    public DbSet<Education> Educations => Set<Education>();
    public DbSet<CV> CVs => Set<CV>();
    public DbSet<StudentLog> StudentLogs => Set<StudentLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLoggerFactory(_loggerFactory)
            .AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
