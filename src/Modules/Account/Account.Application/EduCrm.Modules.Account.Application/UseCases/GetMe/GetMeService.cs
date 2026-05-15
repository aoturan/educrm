using EduCrm.Modules.Account.Application.Helpers;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetMe;

public sealed class GetMeService(ICurrentUserSnapshot currentUser) : IGetMeService
{
    public Task<Result<GetMeResult>> GetMeAsync(CancellationToken ct)
    {
        var initials = NameHelper.GetInitials(currentUser.FullName);

        var roleLabel = currentUser.IsApplicationAdmin
            ? RoleLabel.ApplicationAdmin
            : currentUser.Role.ToString();

        return Task.FromResult(Result<GetMeResult>.Success(new GetMeResult(
            currentUser.Email,
            currentUser.FullName,
            initials,
            roleLabel)));
    }
}