using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CompanyLinkConfigurations : IEntityTypeConfiguration<CompanyLink>
{
    public void Configure(EntityTypeBuilder<CompanyLink> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.Uri)
            .IsRequired();
    }
}
