using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.GetById;

public interface IGetPersonByIdService
{
    Task<Result<GetPersonByIdResult>> GetAsync(Guid personId, CancellationToken ct);
}
