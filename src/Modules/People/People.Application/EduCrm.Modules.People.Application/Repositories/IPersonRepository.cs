using EduCrm.Modules.People.Domain.Entities;

namespace EduCrm.Modules.People.Application.Repositories;

public interface IPersonRepository
{
    void Add(Person person);
    Task<Person?> GetByEmailAsync(string email, CancellationToken ct);
    Task<Person?> GetByUserIdAsync(Guid userId, CancellationToken ct);
}