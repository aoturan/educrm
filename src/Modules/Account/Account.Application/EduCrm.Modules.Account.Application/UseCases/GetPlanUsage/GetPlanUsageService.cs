using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Contracts.Abstractions;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.Modules.People.Contracts.Abstractions;
using EduCrm.Modules.Program.Contracts.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetPlanUsage;

public sealed class GetPlanUsageService(
    IUserRepository userRepo,
    IPlanUsageResolver planUsageResolver,
    IProgramReader programReader,
    IPersonReader personReader,
    IFollowUpReader followUpReader)
    : IGetPlanUsageService
{
    public async Task<Result<GetPlanUsageResult>> GetAsync(GetPlanUsageInput input, CancellationToken ct)
    {
        var caller = await userRepo.GetByIdAsync(input.CallerUserId, ct);
        if (caller is null)
            return Result<GetPlanUsageResult>.Fail(AccountErrors.NotFound(input.CallerUserId));

        if (caller.OrganizationId != input.CallerOrganizationId)
            return Result<GetPlanUsageResult>.Fail(AccountErrors.UserNotInOrganization());

        if (caller.Status != UserStatus.Active)
            return Result<GetPlanUsageResult>.Fail(AccountErrors.UserInactive());

        if (caller.Role != UserRole.Admin)
            return Result<GetPlanUsageResult>.Fail(AccountErrors.NotAdmin());

        var snapshot = await planUsageResolver.ResolveAsync(caller.OrganizationId, ct);

        var userCount = await userRepo.CountActiveByOrganizationAsync(caller.OrganizationId, ct);
        var personCount = await personReader.CountActiveByOrganizationAsync(caller.OrganizationId, ct);
        var programCount = await programReader.CountActiveByOrganizationAsync(caller.OrganizationId, ct);
        var followUpCount = await followUpReader.CountOpenAsync(caller.OrganizationId, ct);

        var pending = snapshot.PendingRequest is null
            ? null
            : new PendingRequestResult(
                snapshot.PendingRequest.RequestedPlanCode,
                snapshot.PendingRequest.Status,
                snapshot.PendingRequest.PaymentMethod,
                snapshot.PendingRequest.Amount,
                snapshot.PendingRequest.PaymentReferenceCode,
                snapshot.PendingRequest.RequestedAtUtc,
                snapshot.PendingRequest.HasPaymentNotification);

        return Result<GetPlanUsageResult>.Success(new GetPlanUsageResult(
            snapshot.PlanCode,
            snapshot.Status,
            snapshot.StartsAtUtc,
            snapshot.EndsAtUtc,
            snapshot.DowngradedFromPlanCode,
            snapshot.DowngradedAtUtc,
            snapshot.Limits.ExportEnabled,
            pending,
            new LimitUsageResult(snapshot.Limits.Users,          userCount),
            new LimitUsageResult(snapshot.Limits.ActivePersons,  personCount),
            new LimitUsageResult(snapshot.Limits.ActivePrograms, programCount),
            new LimitUsageResult(snapshot.Limits.OpenFollowUps,  followUpCount)));
    }
}