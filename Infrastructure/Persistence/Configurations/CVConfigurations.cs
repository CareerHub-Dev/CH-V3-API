using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CVConfigurations : IEntityTypeConfiguration<CV>
{
    public void Configure(EntityTypeBuilder<CV> builder)
    {
        builder.Property(x => x.Title)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.Goals)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(x => x.SkillsAndTechnologies)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(x => x.OtherExperience)
            .HasMaxLength(1024)
            .IsRequired();
    }
}
