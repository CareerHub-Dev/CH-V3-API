using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class StudentSubscriptionConfigurations : IEntityTypeConfiguration<StudentSubscription>
{
    public void Configure(EntityTypeBuilder<StudentSubscription> builder)
    {
        builder
            .HasKey(x => new { x.SubscriptionOwnerId, x.SubscriptionTargetId });

        builder
            .HasOne(s => s.SubscriptionOwner)
            .WithMany(x => x.StudentSubscriptions)
            .HasForeignKey(s => s.SubscriptionOwnerId);

        builder
            .HasOne(s => s.SubscriptionTarget)
            .WithMany(x => x.StudentsSubscribed)
            .HasForeignKey(s => s.SubscriptionTargetId);
    }
}
