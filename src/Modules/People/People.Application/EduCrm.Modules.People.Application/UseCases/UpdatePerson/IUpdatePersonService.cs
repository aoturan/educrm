using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.UpdatePerson;

public interface IUpdatePersonService
{
    Task<Result<UpdatePersonResult>> UpdateAsync(UpdatePersonInput input, CancellationToken ct);
}

