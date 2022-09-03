using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ForeignLanguageConfigurations : IEntityTypeConfiguration<ForeignLanguage>
{
    public void Configure(EntityTypeBuilder<ForeignLanguage> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(32)
            .IsRequired();
    }
}
