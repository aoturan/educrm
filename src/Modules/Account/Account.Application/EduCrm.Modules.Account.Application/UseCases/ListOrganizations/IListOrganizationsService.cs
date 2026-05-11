using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ListOrganizations;

public interface IListOrganizationsService
{
    Task<Result<ListOrganizationsPagedResult>> ListAsync(ListOrganizationsInput input, CancellationToken ct);
}
