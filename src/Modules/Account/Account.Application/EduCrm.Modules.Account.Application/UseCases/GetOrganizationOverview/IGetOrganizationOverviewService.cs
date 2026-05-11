using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetOrganizationOverview;

public interface IGetOrganizationOverviewService
{
    Task<Result<GetOrganizationOverviewResult>> GetAsync(GetOrganizationOverviewInput input, CancellationToken ct);
}
