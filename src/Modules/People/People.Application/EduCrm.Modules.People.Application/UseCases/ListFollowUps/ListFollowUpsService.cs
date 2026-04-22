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

        var onlyOverDue = string.Equals(input.Face, FollowUpFace.OverDue, StringComparison.OrdinalIgnoreCase);

        int page, pageSize;
        if (input.IsBrief)
        {
            page = 1;
            pageSize = 5;
        }
        else
        {
            page = input.Page;
            pageSize = input.PageSize;
        }

        var (items, totalCount) = await followUpRepo.GetListAsync(
            orgContext.OrganizationId.Value,
            page,
            pageSize,
            ct,
            input.PreFilter,
            input.Status,
            input.PersonId,
            input.ProgramId,
            onlyOverDue);

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
