using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ListOrganizations;

public sealed class ListOrganizationsService(
    IOrganizationRepository organizationRepo,
    IClock clock) : IListOrganizationsService
{
    public async Task<Result<ListOrganizationsPagedResult>> ListAsync(ListOrganizationsInput input, CancellationToken ct)
    {
        var queryResult = await organizationRepo.GetPagedListAsync(
            input.Page,
            input.PageSize,
            clock.UtcNow.UtcDateTime,
            ct,
            input.SearchTerm,
            input.Face);

        return Result<ListOrganizationsPagedResult>.Success(new ListOrganizationsPagedResult(
            queryResult.Items.Select(x => new ListOrganizationsItemResult(
                x.Id,
                x.Name,
                x.ContactName,
                x.ContactEmail,
                x.ContactPhone,
                x.CreatedAtUtc)).ToList(),
            input.Page,
            input.PageSize,
            queryResult.TotalCount));
    }
}
