using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetOrganization;

public interface IGetOrganizationService
{
    Task<Result<GetOrganizationResult>> GetAsync(CancellationToken ct);
}
