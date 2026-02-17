using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.Register;

public interface IRegisterService
{
    Task<Result<RegisterResult>> RegisterAsync(
        RegisterInput input,
        CancellationToken ct);
}