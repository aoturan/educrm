using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.People.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduCrm.Infrastructure.Configuration;

public sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> b)
    {
        // Enum -> smallint
        b.Property(x => x.Type)
            .HasConversion<short>();

        b.Property(x => x.Status)
            .HasConversion<short>();
        
        b.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(p => p.AccountOrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
        
        b.HasOne<User>()
            .WithOne()
            .HasForeignKey<Person>(p => p.AccountUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}