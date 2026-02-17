using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ChangePassword;

public interface IChangePasswordService
{
    Task<Result> ChangePasswordAsync(ChangePasswordInput input, CancellationToken ct);
}

