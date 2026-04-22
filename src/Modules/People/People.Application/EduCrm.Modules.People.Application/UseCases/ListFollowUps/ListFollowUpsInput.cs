using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.Modules.People.Application.UseCases.ListFollowUps;

public static class FollowUpFace
{
    public const string OverDue = "overDue";
}

public sealed record ListFollowUpsInput(
    int Page,
    int PageSize,
    IReadOnlyList<FollowUpType>? PreFilter = null,
    IReadOnlyList<FollowUpStatus>? Status = null,
    Guid? PersonId = null,
    Guid? ProgramId = null,
    bool IsBrief = false,
    string? Face = null);
