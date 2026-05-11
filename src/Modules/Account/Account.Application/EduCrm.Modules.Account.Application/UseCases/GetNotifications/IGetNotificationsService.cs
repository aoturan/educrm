using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetNotifications;

public interface IGetNotificationsService
{
    Task<Result<GetNotificationsResult>> GetAsync(Guid organizationId, CancellationToken ct);
}
