using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.UpsertBillingDetail;

public sealed class UpsertBillingDetailService(
    IOrganizationBillingDetailRepository billingRepo,
    IUnitOfWork uow,
    IClock clock,
    ICurrentUserSnapshot user)
    : IUpsertBillingDetailService
{
    public async Task<Result<UpsertBillingDetailResult>> UpsertAsync(UpsertBillingDetailInput input, CancellationToken ct)
    {
        if (user.Role != UserRole.Admin)
            return Result<UpsertBillingDetailResult>.Fail(AccountErrors.NotAdmin());

        // For Individual billing the TaxOffice is irrelevant — drop it so we don't persist UI noise.
        var taxOffice = input.BillingType == BillingType.Corporate ? input.TaxOffice : null;

        var now = clock.UtcNow.UtcDateTime;

        var existing = await billingRepo.GetTrackedByOrganizationIdAsync(user.OrganizationId, ct);
        OrganizationBillingDetail target;

        if (existing is null)
        {
            target = new OrganizationBillingDetail(
                Guid.NewGuid(),
                user.OrganizationId,
                input.BillingType,
                input.BillingName,
                input.TaxNumber,
                taxOffice,
                input.BillingEmail,
                input.BillingAddress,
                now);

            billingRepo.Add(target);
        }
        else
        {
            existing.Update(
                input.BillingType,
                input.BillingName,
                input.TaxNumber,
                taxOffice,
                input.BillingEmail,
                input.BillingAddress,
                now);

            target = existing;
        }

        await uow.SaveChangesAsync(ct);

        return Result<UpsertBillingDetailResult>.Success(new UpsertBillingDetailResult(
            target.BillingType.ToString(),
            target.BillingName,
            target.TaxNumber,
            target.TaxOffice,
            target.BillingEmail,
            target.BillingAddress));
    }
}
