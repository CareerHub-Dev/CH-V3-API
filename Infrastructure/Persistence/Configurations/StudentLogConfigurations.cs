using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class StudentLogConfigurations : IEntityTypeConfiguration<StudentLog>
{
    public void Configure(EntityTypeBuilder<StudentLog> builder)
    {
        builder.Property(x => x.Email)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(x => x.NormalizedEmail);
        builder.Property(x => x.NormalizedEmail)
            .HasMaxLength(256)
            .IsRequired()
            .HasComputedColumnSql(@"LOWER(TRIM(""Email""))", true);

        builder.Property(x => x.FirstName)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(32)
            .IsRequired();
    }
}
