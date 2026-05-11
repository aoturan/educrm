using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.UpdateOrganization;

public sealed class UpdateOrganizationService(
    IUserRepository userRepo,
    IOrganizationRepository organizationRepo,
    IUnitOfWork uow,
    IClock clock)
    : IUpdateOrganizationService
{
    public async Task<Result<UpdateOrganizationResult>> UpdateAsync(UpdateOrganizationInput input, CancellationToken ct)
    {
        var caller = await userRepo.GetByIdAsync(input.CallerUserId, ct);
        if (caller is null)
            return Result<UpdateOrganizationResult>.Fail(AccountErrors.NotFound(input.CallerUserId));

        if (caller.OrganizationId != input.CallerOrganizationId)
            return Result<UpdateOrganizationResult>.Fail(AccountErrors.UserNotInOrganization());

        if (caller.Status != UserStatus.Active)
            return Result<UpdateOrganizationResult>.Fail(AccountErrors.UserInactive());

        if (caller.Role != UserRole.Admin)
            return Result<UpdateOrganizationResult>.Fail(AccountErrors.NotAdmin());

        var organization = await organizationRepo.GetByIdAsync(input.CallerOrganizationId, ct);
        if (organization is null)
            return Result<UpdateOrganizationResult>.Fail(AccountErrors.UserNotInOrganization());

        var now = clock.UtcNow.UtcDateTime;

        organization.Rename(input.OrganizationName, now);
        organization.ChangeContactInfo(input.ContactName, input.ContactEmail, input.ContactPhone, now);

        await uow.SaveChangesAsync(ct);

        return Result<UpdateOrganizationResult>.Success(new UpdateOrganizationResult(
            organization.Id,
            organization.Name,
            organization.ContactName,
            organization.ContactEmail,
            organization.ContactPhone));
    }
}
