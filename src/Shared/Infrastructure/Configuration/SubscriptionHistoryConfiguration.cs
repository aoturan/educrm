using EduCrm.Modules.Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduCrm.Infrastructure.Configuration;

public sealed class SubscriptionHistoryConfiguration : IEntityTypeConfiguration<SubscriptionHistory>
{
    public void Configure(EntityTypeBuilder<SubscriptionHistory> b)
    {
        b.Property(x => x.PlanCode)
            .HasConversion<string>()
            .HasMaxLength(20);

        b.Property(x => x.PaymentMethod)
            .HasConversion<string?>()
            .HasMaxLength(30);
    }
}
