using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetAdminDashboard;

public interface IGetAdminDashboardService
{
    Task<Result<GetAdminDashboardResult>> GetAsync(CancellationToken ct);
}
