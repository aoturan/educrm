using EduCrm.Modules.Support.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduCrm.Infrastructure.Configuration;

public sealed class SupportContactMessageConfiguration : IEntityTypeConfiguration<SupportContactMessage>
{
    public void Configure(EntityTypeBuilder<SupportContactMessage> b)
    {
        b.Property(x => x.FullName).HasMaxLength(200);
        b.Property(x => x.Email).HasMaxLength(320);
        b.Property(x => x.Subject).HasMaxLength(200);
        b.Property(x => x.Status).HasMaxLength(30).HasConversion<string>();
    }
}
