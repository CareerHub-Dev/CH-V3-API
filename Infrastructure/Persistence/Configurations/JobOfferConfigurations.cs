using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class JobOfferConfigurations : IEntityTypeConfiguration<JobOffer>
{
    public void Configure(EntityTypeBuilder<JobOffer> builder)
    {
        builder.Property(x => x.Title)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.Overview)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(x => x.Requirements)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(x => x.Responsibilities)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(x => x.Preferences)
            .HasMaxLength(1024)
            .IsRequired();
    }
}
