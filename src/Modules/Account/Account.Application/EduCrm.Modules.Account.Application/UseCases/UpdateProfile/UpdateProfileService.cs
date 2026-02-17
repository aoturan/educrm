using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Helpers;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.UpdateProfile;

public sealed class UpdateProfileService(
    IUserRepository userRepo,
    IOrganizationRepository orgRepo,
    IUnitOfWork uow,
    IAppDbTransaction tx,
    IClock clock) : IUpdateProfileService
{
    public async Task<Result<UpdateProfileResult>> UpdateProfileAsync(UpdateProfileInput input, CancellationToken ct)
    {
        var now = clock.UtcNow.UtcDateTime;

        // 1) Get user
        var user = await userRepo.GetByIdAsync(input.UserId, ct);
        if (user is null)
        {
            return Result<UpdateProfileResult>.Fail(AccountErrors.NotFound(input.UserId));
        }

        // 2) Check if email changed and is already taken by another user
        if (!string.Equals(user.Email, input.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existingUser = await userRepo.GetByEmailAsync(input.Email, ct);
            if (existingUser is not null && existingUser.Id != input.UserId)
            {
                return Result<UpdateProfileResult>.Fail(AccountErrors.EmailTaken(input.Email));
            }
        }

        // 3) Get organization
        var organization = await orgRepo.GetByIdAsync(input.OrganizationId, ct);
        if (organization is null)
        {
            return Result<UpdateProfileResult>.Fail(AccountErrors.NotFound(input.UserId));
        }

        await using var trx = await tx.BeginAsync(ct);

        try
        {
            // 4) Update User (email and fullName)
            user.ChangeEmail(input.Email, now);
            user.ChangeFullName(input.FullName, now);

            // 5) Update Organization name
            organization.Rename(input.OrganizationName, now);

            // 6) Save all changes
            await uow.SaveChangesAsync(ct);
            await trx.CommitAsync(ct);

            var initials = NameHelper.GetInitials(input.FullName);

            return Result<UpdateProfileResult>.Success(new UpdateProfileResult(
                input.Email,
                input.FullName,
                initials,
                input.OrganizationName));
        }
        catch
        {
            await trx.RollbackAsync(ct);
            throw;
        }
    }
}
