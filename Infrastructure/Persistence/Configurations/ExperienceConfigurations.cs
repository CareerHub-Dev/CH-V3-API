using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ExperienceConfigurations : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        builder.Property(x => x.Title)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.CompanyName)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.JobLocation)
            .HasMaxLength(64)
            .IsRequired();
    }
}
