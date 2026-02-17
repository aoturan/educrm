using EduCrm.Infrastructure.Data;
using EduCrm.Modules.People.Application.Repositories;
using EduCrm.Modules.People.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Modules.People.Infrastructure.Repositories;

public sealed class PersonRepository : IPersonRepository
{
    private readonly AppDbContext _db;

    public PersonRepository(AppDbContext db)
    {
        _db = db;
    }

    public void Add(Person person)
    {
        _db.Persons.Add(person);
    }

    public async Task<Person?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await _db.Persons
            .FirstOrDefaultAsync(p => p.Email == email, ct);
    }

    public async Task<Person?> GetByUserIdAsync(Guid userId, CancellationToken ct)
    {
        return await _db.Persons
            .FirstOrDefaultAsync(p => p.AccountUserId == userId, ct);
    }
}