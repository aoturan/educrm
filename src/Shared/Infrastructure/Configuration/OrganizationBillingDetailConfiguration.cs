using EduCrm.Modules.Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduCrm.Infrastructure.Configuration;

public sealed class OrganizationBillingDetailConfiguration : IEntityTypeConfiguration<OrganizationBillingDetail>
{
    public void Configure(EntityTypeBuilder<OrganizationBillingDetail> b)
    {
        b.Property(x => x.BillingType)
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}