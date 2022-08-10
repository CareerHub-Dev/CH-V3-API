using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CompanyConfigurations : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.Property(x => x.CompanyName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.CompanyMotto)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.CompanyDescription)
            .HasMaxLength(256)
            .IsRequired();
    }
}
