using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.GetDashboardCounts;

public interface IGetDashboardCountsService
{
    Task<Result<GetDashboardCountsResult>> GetAsync(CancellationToken ct);
}

