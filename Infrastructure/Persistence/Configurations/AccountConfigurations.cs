using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AccountConfigurations : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.Property(x => x.Email)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(x => x.NormalizedEmail);
        builder.Property(x => x.NormalizedEmail)
            .HasMaxLength(256)
            .IsRequired()
            .HasComputedColumnSql(@"LOWER(TRIM(""Email""))", true);

        builder.Property(x => x.PasswordHash)
            .IsRequired();
    }
}
