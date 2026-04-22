using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.CreateApplication;

public interface ICreateApplicationService
{
    Task<Result<CreateApplicationResult>> CreateAsync(CreateApplicationInput input, CancellationToken ct);
}

