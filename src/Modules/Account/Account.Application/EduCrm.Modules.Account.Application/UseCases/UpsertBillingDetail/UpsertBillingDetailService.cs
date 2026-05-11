using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.UpsertBillingDetail;

public sealed class UpsertBillingDetailService(
    IUserRepository userRepo,
    IOrganizationBillingDetailRepository billingRepo,
    IUnitOfWork uow,
    IClock clock)
    : IUpsertBillingDetailService
{
    public async Task<Result<UpsertBillingDetailResult>> UpsertAsync(UpsertBillingDetailInput input, CancellationToken ct)
    {
        var caller = await userRepo.GetByIdAsync(input.CallerUserId, ct);
        if (caller is null)
            return Result<UpsertBillingDetailResult>.Fail(AccountErrors.NotFound(input.CallerUserId));

        if (caller.OrganizationId != input.CallerOrganizationId)
            return Result<UpsertBillingDetailResult>.Fail(AccountErrors.UserNotInOrganization());

        if (caller.Status != UserStatus.Active)
            return Result<UpsertBillingDetailResult>.Fail(AccountErrors.UserInactive());

        if (caller.Role != UserRole.Admin)
            return Result<UpsertBillingDetailResult>.Fail(AccountErrors.NotAdmin());

        // For Individual billing the TaxOffice is irrelevant — drop it so we don't persist UI noise.
        var taxOffice = input.BillingType == BillingType.Corporate ? input.TaxOffice : null;

        var now = clock.UtcNow.UtcDateTime;

        var existing = await billingRepo.GetTrackedByOrganizationIdAsync(caller.OrganizationId, ct);
        OrganizationBillingDetail target;

        if (existing is null)
        {
            target = new OrganizationBillingDetail(
                Guid.NewGuid(),
                caller.OrganizationId,
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