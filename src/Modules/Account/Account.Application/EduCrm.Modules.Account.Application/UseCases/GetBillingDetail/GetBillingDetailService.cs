using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetBillingDetail;

public sealed class GetBillingDetailService(
    IUserRepository userRepo,
    IOrganizationBillingDetailRepository billingRepo)
    : IGetBillingDetailService
{
    public async Task<Result<GetBillingDetailResult>> GetAsync(GetBillingDetailInput input, CancellationToken ct)
    {
        var caller = await userRepo.GetByIdAsync(input.CallerUserId, ct);
        if (caller is null)
            return Result<GetBillingDetailResult>.Fail(AccountErrors.NotFound(input.CallerUserId));

        if (caller.OrganizationId != input.CallerOrganizationId)
            return Result<GetBillingDetailResult>.Fail(AccountErrors.UserNotInOrganization());

        if (caller.Status != UserStatus.Active)
            return Result<GetBillingDetailResult>.Fail(AccountErrors.UserInactive());

        if (caller.Role != UserRole.Admin)
            return Result<GetBillingDetailResult>.Fail(AccountErrors.NotAdmin());

        var billing = await billingRepo.GetByOrganizationIdAsync(caller.OrganizationId, ct);
        if (billing is null)
            return Result<GetBillingDetailResult>.Fail(AccountErrors.BillingDetailsNotConfigured());

        return Result<GetBillingDetailResult>.Success(new GetBillingDetailResult(
            billing.BillingType.ToString(),
            billing.BillingName,
            billing.TaxNumber,
            billing.TaxOffice,
            billing.BillingEmail,
            billing.BillingAddress));
    }
}