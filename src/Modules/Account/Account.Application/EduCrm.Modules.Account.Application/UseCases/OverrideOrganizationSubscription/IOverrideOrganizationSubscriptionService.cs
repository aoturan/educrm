using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.OverrideOrganizationSubscription;

public interface IOverrideOrganizationSubscriptionService
{
    Task<Result<OverrideOrganizationSubscriptionResult>> OverrideAsync(
        OverrideOrganizationSubscriptionInput input,
        CancellationToken ct);
}
