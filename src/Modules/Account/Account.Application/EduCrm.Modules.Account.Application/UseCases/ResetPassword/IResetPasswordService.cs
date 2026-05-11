using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ResetPassword;

public interface IResetPasswordService
{
    Task<Result> ResetAsync(ResetPasswordInput input, CancellationToken ct);
}
