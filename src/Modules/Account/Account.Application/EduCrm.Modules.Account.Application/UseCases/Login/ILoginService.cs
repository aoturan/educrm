using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.Login;

public interface ILoginService
{
    Task<Result<LoginResult>> LoginAsync(LoginInput input, CancellationToken ct);
}
