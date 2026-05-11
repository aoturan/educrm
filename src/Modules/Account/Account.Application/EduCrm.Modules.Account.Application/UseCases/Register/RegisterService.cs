using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Helpers;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Application.Security;
using EduCrm.Modules.Account.Application.SubscriptionRequests;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.Register;

public sealed class RegisterService(
    IOrganizationRepository orgRepo,
    IUserRepository userRepo,
    ISubscriptionRepository subscriptionRepo,
    ISubscriptionRequestRepository subscriptionRequestRepo,
    IPlanPricingResolver planPricingResolver,
    IPaymentReferenceCodeGenerator referenceCodeGenerator,
    IUnitOfWork uow,
    IAppDbTransaction tx,
    IClock clock,
    IJwtService jwtService)
    : IRegisterService
{
    private static readonly TimeSpan SubscriptionRequestLifetime = TimeSpan.FromDays(7);

    public async Task<Result<RegisterResult>> RegisterAsync(
        RegisterInput input,
        CancellationToken ct)
    {
        // Check if email already exists
        var existingUser = await userRepo.GetByEmailAsync(input.Email, ct);
        if (existingUser is not null)
        {
            return Result<RegisterResult>.Fail(AccountErrors.EmailTaken(input.Email));
        }

        await using var trx = await tx.BeginAsync(ct);

        try
        {
            var normalizedPhone = PhoneNormalizer.Normalize(input.Phone);
            if (normalizedPhone is null)
                return Result<RegisterResult>.Fail(AccountErrors.InvalidPhoneFormat());

            var now = clock.UtcNow.UtcDateTime;
            var accountId = Guid.NewGuid();

            // 1) Organization
            var organization = new Organization(
                accountId,
                input.OrganizationName,
                now,
                input.Name,
                input.Email,
                normalizedPhone);

            orgRepo.Add(organization);

            // 2) User
            var user = new User(
                Guid.NewGuid(),
                accountId,
                input.Email,
                input.Name,
                input.PasswordHash,
                UserRole.Admin,
                now);

            userRepo.Add(user);

            // Registration auto-logs the user in (redirects to dashboard)
            user.Enable(now);
            user.SetLastLogin(now);

            // 3) Subscription. Paid signups start on Free; the SubscriptionRequest
            //    below tracks the chosen plan and gets approved on payment.
            var isPaidPlan = input.PlanCode is PlanCode.Plus or PlanCode.Pro;

            var subscription = new Subscription(
                Guid.NewGuid(),
                accountId,
                isPaidPlan ? PlanCode.Free : input.PlanCode,
                now,
                endsAtUtc: null,
                now);

            subscriptionRepo.Add(subscription);

            // 4) Pending payment request mirrors the upgrade-from-dashboard flow.
            if (isPaidPlan)
            {
                var amount = planPricingResolver.GetPrice(input.PlanCode);
                var referenceCode = await referenceCodeGenerator.GenerateAsync(ct);

                var request = new SubscriptionRequest(
                    Guid.NewGuid(),
                    accountId,
                    input.PlanCode,
                    RequestStatus.PendingPayment,
                    PaymentMethod.BankTransfer,
                    amount,
                    referenceCode,
                    now,
                    now.Add(SubscriptionRequestLifetime),
                    now);

                subscriptionRequestRepo.Add(request);
            }

            // single commit
            await uow.SaveChangesAsync(ct);
            await trx.CommitAsync(ct);

            // Generate JWT token
            var token = jwtService.GenerateToken(user.Id, user.IsApplicationAdmin);

            var initials = NameHelper.GetInitials(input.Name);

            return Result<RegisterResult>.Success(new RegisterResult(
                token,
                input.Email,
                input.Name,
                initials,
                input.OrganizationName,
                RoleLabel.Resolve(user)));
        }
        catch
        {
            await trx.RollbackAsync(ct);
            throw;
        }
    }

}