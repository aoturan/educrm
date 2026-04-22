using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.ContactApplication;

public sealed record ContactApplicationInput(Guid ApplicationId, Guid PersonId);

public interface IContactApplicationService
{
    Task<Result> ContactAsync(ContactApplicationInput input, CancellationToken ct);
}

