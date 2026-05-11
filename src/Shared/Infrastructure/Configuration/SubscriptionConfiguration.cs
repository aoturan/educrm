using EduCrm.Modules.Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduCrm.Infrastructure.Configuration;

public sealed class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> b)
    {
        b.Property(x => x.PlanCode)
            .HasConversion<string>()
            .HasMaxLength(20);

        b.Property(x => x.DowngradedFromPlanCode)
            .HasConversion<string?>()
            .HasMaxLength(20);

        b.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.OrganizationId);
    }
}
