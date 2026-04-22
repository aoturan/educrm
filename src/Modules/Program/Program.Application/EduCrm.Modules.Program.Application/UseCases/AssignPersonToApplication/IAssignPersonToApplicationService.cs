using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.AssignPersonToApplication;

public interface IAssignPersonToApplicationService
{
    Task<Result<AssignPersonResult>> AssignAsync(AssignPersonInput input, CancellationToken ct);
}

