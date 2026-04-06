using EduCrm.Modules.People.Application.Errors;
using EduCrm.Modules.People.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.GetFollowUpById;

public sealed class GetFollowUpByIdService(
    IFollowUpRepository followUpRepo,
    IOrgContext orgContext) : IGetFollowUpByIdService
{
    public async Task<Result<GetFollowUpByIdResult>> GetAsync(Guid followUpId, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
            return Result<GetFollowUpByIdResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var data = await followUpRepo.GetByIdAsync(followUpId, orgContext.OrganizationId.Value, ct);

        if (data is null)
            return Result<GetFollowUpByIdResult>.Fail(PeopleErrors.FollowUpNotFound(followUpId));

        return Result<GetFollowUpByIdResult>.Success(new GetFollowUpByIdResult(
            data.Id,
            data.OrganizationId,
            data.Type,
            data.Status,
            data.Title,
            data.Note,
            data.DueAtUtc,
            data.SnoozedUntilUtc,
            data.CompletedAtUtc,
            data.CancelledAtUtc,
            new GetFollowUpByIdPersonResult(
                data.Person.Id,
                data.Person.FullName,
                data.Person.Email,
                data.Person.Phone),
            data.Program is null ? null : new GetFollowUpByIdProgramResult(
                data.Program.Id,
                data.Program.Name)));
    }
}
