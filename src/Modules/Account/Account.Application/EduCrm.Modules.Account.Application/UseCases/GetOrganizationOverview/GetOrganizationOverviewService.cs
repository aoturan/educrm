using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Contracts.Abstractions;
using EduCrm.Modules.People.Contracts.Abstractions;
using EduCrm.Modules.Program.Contracts.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetOrganizationOverview;

public sealed class GetOrganizationOverviewService(
    IOrganizationRepository organizationRepo,
    IUserRepository userRepo,
    ISubscriptionRepository subscriptionRepo,
    ISubscriptionRequestRepository subscriptionRequestRepo,
    IOrganizationBillingDetailRepository billingDetailRepo,
    IPlanUsageResolver planUsageResolver,
    IProgramReader programReader,
    IPersonReader personReader,
    IFollowUpReader followUpReader)
    : IGetOrganizationOverviewService
{
    public async Task<Result<GetOrganizationOverviewResult>> GetAsync(GetOrganizationOverviewInput input, CancellationToken ct)
    {
        var organization = await organizationRepo.GetByIdAsync(input.OrganizationId, ct);
        if (organization is null)
            return Result<GetOrganizationOverviewResult>.Fail(AccountErrors.OrganizationNotFound(input.OrganizationId));

        var owner = await userRepo.GetOrganizationOwnerAsync(input.OrganizationId, ct);

        var snapshot = await planUsageResolver.ResolveAsync(input.OrganizationId, ct);

        var userCount = await userRepo.CountActiveByOrganizationAsync(input.OrganizationId, ct);
        var personCount = await personReader.CountActiveByOrganizationAsync(input.OrganizationId, ct);
        var programCount = await programReader.CountActiveByOrganizationAsync(input.OrganizationId, ct);
        var followUpCount = await followUpReader.CountOpenAsync(input.OrganizationId, ct);

        var subscription = await subscriptionRepo.GetCurrentByOrganizationAsync(input.OrganizationId, ct);
        var subscriptionResult = subscription is null
            ? null
            : new OrganizationOverviewSubscriptionResult(
                subscription.Id,
                subscription.PlanCode.ToString(),
                subscription.StartsAtUtc,
                subscription.EndsAtUtc,
                subscription.DowngradedFromPlanCode?.ToString(),
                subscription.DowngradedAtUtc,
                subscription.CreatedAtUtc,
                subscription.UpdatedAtUtc);

        var billingDetail = await billingDetailRepo.GetByOrganizationIdAsync(input.OrganizationId, ct);
        var billingDetailResult = billingDetail is null
            ? null
            : new OrganizationOverviewBillingDetailResult(
                billingDetail.Id,
                billingDetail.BillingType.ToString(),
                billingDetail.BillingName,
                billingDetail.TaxNumber,
                billingDetail.TaxOffice,
                billingDetail.BillingEmail,
                billingDetail.BillingAddress,
                billingDetail.CreatedAtUtc,
                billingDetail.UpdatedAtUtc);

        var requests = await subscriptionRequestRepo.GetByOrganizationAsync(input.OrganizationId, ct);
        var requestResults = requests
            .Select(r => new OrganizationOverviewSubscriptionRequestResult(
                r.Id,
                r.RequestedPlanCode.ToString(),
                r.Status.ToString(),
                r.PaymentMethod.ToString(),
                r.Amount,
                r.PaymentReferenceCode,
                r.RequestedAtUtc,
                r.ExpiresAtUtc,
                r.ApprovedAtUtc,
                r.RejectedAtUtc,
                r.CancelledAtUtc,
                r.IsInvoiced,
                r.HasPaymentNotification))
            .ToList();

        var limits = new OrganizationOverviewLimitsResult(
            snapshot.PlanCode,
            snapshot.Status,
            snapshot.Limits.ExportEnabled,
            new LimitUsageResult(snapshot.Limits.Users, userCount),
            new LimitUsageResult(snapshot.Limits.ActivePersons, personCount),
            new LimitUsageResult(snapshot.Limits.ActivePrograms, programCount),
            new LimitUsageResult(snapshot.Limits.OpenFollowUps, followUpCount));

        return Result<GetOrganizationOverviewResult>.Success(new GetOrganizationOverviewResult(
            organization.Id,
            organization.Name,
            owner!.FullName,
            owner.Email,
            organization.ContactPhone,
            organization.CreatedAtUtc,
            limits,
            subscriptionResult,
            billingDetailResult,
            requestResults));
    }
}
