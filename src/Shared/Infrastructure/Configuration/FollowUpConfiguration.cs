using EduCrm.Modules.People.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduCrm.Infrastructure.Configuration;

public sealed class FollowUpConfiguration : IEntityTypeConfiguration<FollowUp>
{
    public void Configure(EntityTypeBuilder<FollowUp> b)
    {
        // Enum -> string
        b.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(20);

        b.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}


