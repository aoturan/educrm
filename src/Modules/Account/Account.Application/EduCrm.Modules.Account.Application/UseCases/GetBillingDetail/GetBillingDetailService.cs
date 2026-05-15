using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetBillingDetail;

public sealed class GetBillingDetailService(
    IOrganizationBillingDetailRepository billingRepo,
    ICurrentUserSnapshot user)
    : IGetBillingDetailService
{
    public async Task<Result<GetBillingDetailResult>> GetAsync(CancellationToken ct)
    {
        if (user.Role != UserRole.Admin)
            return Result<GetBillingDetailResult>.Fail(AccountErrors.NotAdmin());

        var billing = await billingRepo.GetByOrganizationIdAsync(user.OrganizationId, ct);
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
