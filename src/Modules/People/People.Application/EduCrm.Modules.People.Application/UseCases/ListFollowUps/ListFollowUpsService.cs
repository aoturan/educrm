using EduCrm.Modules.People.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.ListFollowUps;

public sealed class ListFollowUpsService(
    IFollowUpRepository followUpRepo,
    IOrgContext orgContext) : IListFollowUpsService
{
    public async Task<Result<ListFollowUpsPagedResult>> ListAsync(ListFollowUpsInput input, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
            return Result<ListFollowUpsPagedResult>.Fail(
                CommonErrors.Forbidden("Organization scope is missing."));

        var (items, totalCount) = await followUpRepo.GetListAsync(
            orgContext.OrganizationId.Value,
            input.Page,
            input.PageSize,
            ct,
            input.PreFilter,
            input.Status,
            input.PersonId,
            input.ProgramId);

        var results = items
            .Select(x => new FollowUpListItemResult(
                x.Id,
                x.PersonName,
                x.ProgramName,
                x.Type,
                x.Status,
                x.Title,
                x.DueAtUtc,
                x.SnoozedUntilUtc))
            .ToList();

        return Result<ListFollowUpsPagedResult>.Success(
            new ListFollowUpsPagedResult(results, totalCount));
    }
}

