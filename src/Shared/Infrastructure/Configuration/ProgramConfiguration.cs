using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.Program.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduCrm.Infrastructure.Configuration;

public sealed class ProgramConfiguration : IEntityTypeConfiguration<Program>
{
    public void Configure(EntityTypeBuilder<Program> b)
    {
        // Enum -> string
        b.Property(x => x.Status)
            .HasConversion<string>();

        b.Property(x => x.PublicModality)
            .HasConversion<string>();

        b.Property(x => x.PriceCurrency)
            .HasConversion<string>()
            .HasMaxLength(3);

        // FK: OrganizationId (navigation yok)
        b.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}