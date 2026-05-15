using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetOrganization;

public sealed class GetOrganizationService(
    IOrganizationRepository organizationRepo,
    ICurrentUserSnapshot user)
    : IGetOrganizationService
{
    public async Task<Result<GetOrganizationResult>> GetAsync(CancellationToken ct)
    {
        if (user.Role != UserRole.Admin)
            return Result<GetOrganizationResult>.Fail(AccountErrors.NotAdmin());

        var organization = await organizationRepo.GetByIdAsync(user.OrganizationId, ct);
        if (organization is null)
            return Result<GetOrganizationResult>.Fail(AccountErrors.UserNotInOrganization());

        return Result<GetOrganizationResult>.Success(new GetOrganizationResult(
            organization.Id,
            organization.Name,
            organization.ContactName,
            organization.ContactEmail,
            organization.ContactPhone));
    }
}
