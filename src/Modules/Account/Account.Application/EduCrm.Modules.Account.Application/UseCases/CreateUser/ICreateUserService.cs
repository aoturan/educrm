using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.CreateUser;

public interface ICreateUserService
{
    Task<Result<CreateUserResult>> CreateAsync(
        CreateUserInput input,
        CancellationToken ct);
}