using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.ListPersons;

public interface IListPersonsService
{
    Task<Result<ListPersonsResult>> ListAsync(ListPersonsInput input, CancellationToken ct);
}

