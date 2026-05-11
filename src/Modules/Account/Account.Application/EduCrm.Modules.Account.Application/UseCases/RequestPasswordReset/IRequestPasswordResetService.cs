using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.RequestPasswordReset;

public interface IRequestPasswordResetService
{
    Task<Result> RequestAsync(RequestPasswordResetInput input, CancellationToken ct);
}