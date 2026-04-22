using EduCrm.Modules.Support.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduCrm.Infrastructure.Configuration;

public sealed class SupportRequestConfiguration : IEntityTypeConfiguration<SupportRequest>
{
    public void Configure(EntityTypeBuilder<SupportRequest> b)
    {
        b.Property(x => x.Subject).HasMaxLength(150);
        b.Property(x => x.Message).HasMaxLength(4000);
        b.Property(x => x.PageUrl).HasMaxLength(500);
        b.Property(x => x.Status).HasConversion<string>();
    }
}

