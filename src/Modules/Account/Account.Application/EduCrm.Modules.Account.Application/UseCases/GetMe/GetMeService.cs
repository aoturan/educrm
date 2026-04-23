using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Helpers;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetMe;

public sealed class GetMeService(IUserRepository userRepo) : IGetMeService
{
    public async Task<Result<GetMeResult>> GetMeAsync(Guid userId, CancellationToken ct)
    {
        var user = await userRepo.GetByIdAsync(userId, ct);
        if (user is null)
        {
            return Result<GetMeResult>.Fail(AccountErrors.NotFound(userId));
        }

        var initials = NameHelper.GetInitials(user.FullName);

        return Result<GetMeResult>.Success(new GetMeResult(
            user.Email,
            user.FullName,
            initials,
            user.Role));
    }
}