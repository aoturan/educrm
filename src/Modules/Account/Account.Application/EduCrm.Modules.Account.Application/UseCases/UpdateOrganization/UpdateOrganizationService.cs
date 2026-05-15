using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.UpdateOrganization;

public sealed class UpdateOrganizationService(
    IOrganizationRepository organizationRepo,
    IUnitOfWork uow,
    IClock clock,
    ICurrentUserSnapshot user)
    : IUpdateOrganizationService
{
    public async Task<Result<UpdateOrganizationResult>> UpdateAsync(UpdateOrganizationInput input, CancellationToken ct)
    {
        if (user.Role != UserRole.Admin)
            return Result<UpdateOrganizationResult>.Fail(AccountErrors.NotAdmin());

        var organization = await organizationRepo.GetByIdAsync(user.OrganizationId, ct);
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
