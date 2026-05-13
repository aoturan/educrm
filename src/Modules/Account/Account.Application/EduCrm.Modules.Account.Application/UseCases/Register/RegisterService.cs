using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Email;
using EduCrm.Modules.Account.Application.EmailVerification;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Helpers;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Application.Security;
using EduCrm.Modules.Account.Application.SubscriptionRequests;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;
using Microsoft.Extensions.Options;

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
    IEmailSender emailSender,
    IOptions<EmailVerificationOptions> verificationOptions)
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

            var verificationOpts = verificationOptions.Value;
            var plainVerificationToken = VerificationTokenGenerator.GenerateToken();
            var verificationTokenHash = VerificationTokenGenerator.HashToken(plainVerificationToken);
            var verificationExpiresAt = now.AddMinutes(verificationOpts.TokenLifetimeMinutes);
            user.IssueEmailVerification(verificationTokenHash, verificationExpiresAt, now);

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

            try
            {
                var verificationLink = EmailVerificationEmailBuilder.BuildLink(
                    verificationOpts.VerificationUrl, user.Email, plainVerificationToken);
                var verificationMessage = EmailVerificationEmailBuilder.Build(
                    user.Email, user.FullName, verificationLink, verificationOpts.TokenLifetimeMinutes);
                await emailSender.SendAsync(verificationMessage, ct);
            }
            catch
            {
                // Swallow: delivery failure should not block registration; user can request resend.
            }

            // No JWT is issued at registration; the user must verify their email and then log in.
            return Result<RegisterResult>.Success(new RegisterResult(
                Email: user.Email,
                Status: user.Status,
                Token: null,
                FullName: null,
                Initials: null,
                OrganizationName: null,
                Role: null));
        }
        catch
        {
            await trx.RollbackAsync(ct);
            throw;
        }
    }

}