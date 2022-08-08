using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AccountConfigurations : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasIndex(x => x.NormalizedEmail);

        builder.Property(x => x.NormalizedEmail).HasComputedColumnSql(@"LOWER(TRIM(""Email""))", true);
    }
}
