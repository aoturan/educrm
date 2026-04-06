using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.Modules.People.Application.UseCases.ListFollowUps;

public sealed record ListFollowUpsInput(
    int Page,
    int PageSize,
    IReadOnlyList<FollowUpType>? PreFilter = null,
    IReadOnlyList<FollowUpStatus>? Status = null,
    Guid? PersonId = null,
    Guid? ProgramId = null);

