using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetOrganization;

public sealed class GetOrganizationService(
    IUserRepository userRepo,
    IOrganizationRepository organizationRepo)
    : IGetOrganizationService
{
    public async Task<Result<GetOrganizationResult>> GetAsync(GetOrganizationInput input, CancellationToken ct)
    {
        var caller = await userRepo.GetByIdAsync(input.CallerUserId, ct);
        if (caller is null)
            return Result<GetOrganizationResult>.Fail(AccountErrors.NotFound(input.CallerUserId));

        if (caller.OrganizationId != input.CallerOrganizationId)
            return Result<GetOrganizationResult>.Fail(AccountErrors.UserNotInOrganization());

        if (caller.Role != UserRole.Admin)
            return Result<GetOrganizationResult>.Fail(AccountErrors.NotAdmin());

        var organization = await organizationRepo.GetByIdAsync(input.CallerOrganizationId, ct);
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