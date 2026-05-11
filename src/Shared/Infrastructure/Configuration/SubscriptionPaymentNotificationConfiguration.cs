using EduCrm.Modules.Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduCrm.Infrastructure.Configuration;

public sealed class SubscriptionPaymentNotificationConfiguration : IEntityTypeConfiguration<SubscriptionPaymentNotification>
{
    public void Configure(EntityTypeBuilder<SubscriptionPaymentNotification> b)
    {
        b.Property(x => x.Amount)
            .HasPrecision(18, 2);

        b.HasIndex(x => x.SubscriptionRequestId);
        b.HasIndex(x => x.OrganizationId);
    }
}