using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.PublishProgram;

public interface IPublishProgramService
{
    Task<Result<PublishProgramResult>> PublishAsync(PublishProgramInput input, CancellationToken ct);
}

