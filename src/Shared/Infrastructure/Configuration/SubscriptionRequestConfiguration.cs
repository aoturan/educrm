using EduCrm.Modules.Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduCrm.Infrastructure.Configuration;

public sealed class SubscriptionRequestConfiguration : IEntityTypeConfiguration<SubscriptionRequest>
{
    public void Configure(EntityTypeBuilder<SubscriptionRequest> b)
    {
        b.Property(x => x.RequestedPlanCode)
            .HasConversion<string>()
            .HasMaxLength(20);

        b.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        b.Property(x => x.PaymentMethod)
            .HasConversion<string>()
            .HasMaxLength(30);

        b.Property(x => x.Amount)
            .HasPrecision(18, 2);

        b.HasIndex(x => x.PaymentReferenceCode)
            .IsUnique();
    }
}