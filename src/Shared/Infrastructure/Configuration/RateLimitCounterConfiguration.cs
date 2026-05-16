using EduCrm.Infrastructure.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduCrm.Infrastructure.Configuration;

public sealed class RateLimitCounterConfiguration : IEntityTypeConfiguration<RateLimitCounter>
{
    public void Configure(EntityTypeBuilder<RateLimitCounter> b)
    {
        b.Property(x => x.Action).HasMaxLength(100);
        b.Property(x => x.Key).HasMaxLength(256);

        b.HasIndex(x => new { x.Action, x.Key, x.WindowStartUtc }).IsUnique();
        b.HasIndex(x => x.WindowEndUtc);
    }
}
