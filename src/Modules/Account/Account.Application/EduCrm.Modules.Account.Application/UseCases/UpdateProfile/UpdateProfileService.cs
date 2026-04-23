using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Helpers;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
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

        var emailChanged = !string.Equals(user.Email, input.Email, StringComparison.OrdinalIgnoreCase);
        if (emailChanged)
        {
            if (user.Role != UserRole.Admin)
            {
                return Result<UpdateProfileResult>.Fail(AccountErrors.EmailChangeNotAllowed());
            }

            var existingUser = await userRepo.GetByEmailAsync(input.Email, ct);
            if (existingUser is not null && existingUser.Id != input.UserId)
            {
                return Result<UpdateProfileResult>.Fail(AccountErrors.EmailTaken(input.Email));
            }
        }

        user.ChangeEmail(input.Email, now);
        user.ChangeFullName(input.FullName, now);

        await uow.SaveChangesAsync(ct);

        var initials = NameHelper.GetInitials(input.FullName);

        return Result<UpdateProfileResult>.Success(new UpdateProfileResult(
            input.Email,
            input.FullName,
            initials));
    }
}