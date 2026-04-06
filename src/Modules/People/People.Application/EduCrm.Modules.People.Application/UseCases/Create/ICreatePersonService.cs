using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.Create;

public interface ICreatePersonService
{
    Task<Result<CreateResult>> CreateAsync(CreateInput input, CancellationToken ct);
}

