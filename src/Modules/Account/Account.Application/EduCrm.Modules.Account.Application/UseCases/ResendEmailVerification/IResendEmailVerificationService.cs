using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ResendEmailVerification;

public interface IResendEmailVerificationService
{
    Task<Result> ResendAsync(ResendEmailVerificationInput input, CancellationToken ct);
}
