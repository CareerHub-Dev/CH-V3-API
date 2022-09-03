using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EducationConfigurations : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.Property(x => x.University)
           .HasMaxLength(64)
           .IsRequired();

        builder.Property(x => x.City)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.Country)
           .HasMaxLength(64)
           .IsRequired();

        builder.Property(x => x.Specialty)
            .HasMaxLength(64)
            .IsRequired();
    }
}
