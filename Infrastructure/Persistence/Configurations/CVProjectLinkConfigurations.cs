using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CVProjectLinkConfigurations : IEntityTypeConfiguration<CVProjectLink>
{
    public void Configure(EntityTypeBuilder<CVProjectLink> builder)
    {
        builder.Property(x => x.Title)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.Url)
            .IsRequired();
    }
}
