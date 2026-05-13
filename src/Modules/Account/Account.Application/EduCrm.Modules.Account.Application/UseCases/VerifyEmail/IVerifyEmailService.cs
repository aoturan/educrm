using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.VerifyEmail;

public interface IVerifyEmailService
{
    Task<Result> VerifyAsync(VerifyEmailInput input, CancellationToken ct);
}
