using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Helpers;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.UpdateProfile;

public sealed class UpdateProfileService(
    IUserRepository userRepo,
    IUnitOfWork uow,
    IClock clock) : IUpdateProfileService
{
    public async Task<Result<UpdateProfileResult>> UpdateProfileAsync(UpdateProfileInput input, CancellationToken ct)
    {
        var now = clock.UtcNow.UtcDateTime;

        var user = await userRepo.GetByIdAsync(input.UserId, ct);
        if (user is null)
        {
            return Result<UpdateProfileResult>.Fail(AccountErrors.NotFound(input.UserId));
        }

        if (user.OrganizationId != input.OrganizationId)
        {
            return Result<UpdateProfileResult>.Fail(AccountErrors.UserNotInOrganization());
        }

        user.ChangeFullName(input.FullName, now);

        await uow.SaveChangesAsync(ct);

        var initials = NameHelper.GetInitials(user.FullName);

        return Result<UpdateProfileResult>.Success(new UpdateProfileResult(
            user.Email,
            user.FullName,
            initials));
    }
}