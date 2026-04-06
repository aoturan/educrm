using EduCrm.Modules.People.Domain.Enums;
using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Person;

public sealed record PersonEnrolledProgramResponse(
    Guid Id,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    ProgramStatus Status);

public sealed record PersonFollowUpResponse(
    string Title,
    FollowUpStatus Status,
    FollowUpType Type,
    DateTime DueAtUtc,
    DateTime? SnoozedUntilUtc,
    string? ProgramName);

public sealed record GetPersonByIdResponse(
    Guid Id,
    string Name,
    string? Email,
    string? Phone,
    DateTime JoinedDate,
    string? Notes,
    IReadOnlyList<PersonEnrolledProgramResponse> EnrolledPrograms,
    IReadOnlyList<PersonFollowUpResponse> FollowUps,
    bool IsArchived,
    DateTime? ArchivedAtUtc);
