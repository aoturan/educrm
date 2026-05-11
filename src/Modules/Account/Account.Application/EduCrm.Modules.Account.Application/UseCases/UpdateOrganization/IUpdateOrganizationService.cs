using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.UpdateOrganization;

public interface IUpdateOrganizationService
{
    Task<Result<UpdateOrganizationResult>> UpdateAsync(UpdateOrganizationInput input, CancellationToken ct);
}
