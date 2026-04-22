using EduCrm.Infrastructure.Data;
using EduCrm.Modules.People.Contracts.Abstractions;
using EduCrm.Modules.People.Domain.Entities;
using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.Modules.People.Infrastructure.Writers;

public sealed class PersonWriter(AppDbContext db) : IPersonWriter
{
    public Guid AddFromApplication(
        Guid organizationId,
        string fullName,
        string? phone,
        string? email,
        DateTime nowUtc)
    {
        var person = new Person(organizationId, fullName, SourceType.Application, nowUtc, phone, email);
        db.Persons.Add(person);
        return person.Id;
    }
}

