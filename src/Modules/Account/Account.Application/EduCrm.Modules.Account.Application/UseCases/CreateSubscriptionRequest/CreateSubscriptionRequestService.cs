using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Application.SubscriptionRequests;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.CreateSubscriptionRequest;

public sealed class CreateSubscriptionRequestService(
    IUserRepository userRepo,
    ISubscriptionRepository subscriptionRepo,
    ISubscriptionRequestRepository subscriptionRequestRepo,
    IPlanPricingResolver planPricingResolver,
    IPaymentReferenceCodeGenerator referenceCodeGenerator,
    IUnitOfWork uow,
    IClock clock)
    : ICreateSubscriptionRequestService
{
    public async Task<Result<CreateSubscriptionRequestResult>> CreateAsync(
        CreateSubscriptionRequestInput input,
        CancellationToken ct)   
    {
        var caller = await userRepo.GetByIdAsync(input.CallerUserId, ct);
        if (caller is null)
            return Result<CreateSubscriptionRequestResult>.Fail(AccountErrors.NotFound(input.CallerUserId));

        if (caller.OrganizationId != input.CallerOrganizationId)
            return Result<CreateSubscriptionRequestResult>.Fail(AccountErrors.UserNotInOrganization());

        if (caller.Status != UserStatus.Active)
            return Result<CreateSubscriptionRequestResult>.Fail(AccountErrors.UserInactive());

        if (caller.Role != UserRole.Admin)
            return Result<CreateSubscriptionRequestResult>.Fail(AccountErrors.NotAdmin());

        var existingActive = await subscriptionRequestRepo.GetActiveByOrganizationAsync(caller.OrganizationId, ct);
        if (existingActive is not null && existingActive.RequestedPlanCode == input.RequestedPlanCode)
            return Result<CreateSubscriptionRequestResult>.Success(ToResult(existingActive));

        var currentSubscription = await subscriptionRepo.GetCurrentByOrganizationAsync(caller.OrganizationId, ct);
        if (currentSubscription is not null && !IsEligibleUpgrade(currentSubscription.PlanCode, input.RequestedPlanCode))
            return Result<CreateSubscriptionRequestResult>.Fail(AccountErrors.SubscriptionPlanNotEligible());

        var now = clock.UtcNow.UtcDateTime;

        if (existingActive is not null)
            existingActive.Cancel(now);

        var amount = planPricingResolver.GetPrice(input.RequestedPlanCode);
        var referenceCode = await referenceCodeGenerator.GenerateAsync(ct);

        var expiresAt = now.AddDays(7);

        var request = new SubscriptionRequest(
            Guid.NewGuid(),
            caller.OrganizationId,
            input.RequestedPlanCode,
            RequestStatus.PendingPayment,
            PaymentMethod.BankTransfer,
            amount,
            referenceCode,
            now,
            expiresAt,
            now);

        subscriptionRequestRepo.Add(request);
        await uow.SaveChangesAsync(ct);

        return Result<CreateSubscriptionRequestResult>.Success(ToResult(request));
    }

    private static CreateSubscriptionRequestResult ToResult(SubscriptionRequest request) =>
        new(
            request.Id,
            request.RequestedPlanCode.ToString(),
            request.Status.ToString(),
            request.PaymentMethod.ToString(),
            request.Amount,
            request.PaymentReferenceCode,
            request.RequestedAtUtc,
            request.ExpiresAtUtc);

    private static bool IsEligibleUpgrade(PlanCode current, PlanCode requested) => current switch
    {
        PlanCode.Free => requested is PlanCode.Plus or PlanCode.Pro,
        PlanCode.Plus => requested is PlanCode.Pro,
        _             => false
    };

}