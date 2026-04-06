using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.Create;

public interface ICreateService
{
    Task<Result<CreateResult>> CreateAsync(CreateInput input, CancellationToken ct);
}

