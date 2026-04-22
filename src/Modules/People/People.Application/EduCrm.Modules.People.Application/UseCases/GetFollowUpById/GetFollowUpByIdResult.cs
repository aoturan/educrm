using EduCrm.Modules.People.Application.Repositories;
using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.Modules.People.Application.UseCases.GetFollowUpById;

public sealed record GetFollowUpByIdPersonResult(Guid Id, string FullName, string? Email, string? Phone);
public sealed record GetFollowUpByIdProgramResult(Guid Id, string Name, DateOnly StartDate, DateOnly EndDate);

public sealed record GetFollowUpByIdResult(
    Guid Id,
    Guid OrganizationId,
    FollowUpType Type,
    FollowUpStatus Status,
    string Title,
    string? Note,
    DateTime DueAtUtc,
    DateTime? SnoozedUntilUtc,
    DateTime? CompletedAtUtc,
    DateTime? CancelledAtUtc,
    GetFollowUpByIdPersonResult Person,
    GetFollowUpByIdProgramResult? Program);
