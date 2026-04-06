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
        b.Property(x => x.Source)
            .HasConversion<string>();
    }
}