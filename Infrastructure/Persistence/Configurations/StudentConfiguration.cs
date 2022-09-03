using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.Property(x => x.FirstName)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasMaxLength(32);
    }
}
