using EduCrm.Modules.People.Application.Repositories;

namespace EduCrm.Modules.People.Application.UseCases.GetById;

public sealed record GetPersonByIdResult(
    Guid Id,
    string FullName,
    string? Email,
    string? Phone,
    DateTime JoinedDate,
    string? Notes,
    IReadOnlyList<PersonEnrolledProgramData> EnrolledPrograms,
    IReadOnlyList<PersonFollowUpData> FollowUps,
    bool IsArchived,
    DateTime? ArchivedAtUtc);
