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

        builder.Property(x => x.ForeignLanguages)
            .HasColumnType("jsonb");

        builder.Property(x => x.ProjectLinks)
            .HasColumnType("jsonb");

        builder.Property(x => x.Educations)
            .HasColumnType("jsonb");

        builder.Property(x => x.Experiences)
            .HasColumnType("jsonb");
    }
}
