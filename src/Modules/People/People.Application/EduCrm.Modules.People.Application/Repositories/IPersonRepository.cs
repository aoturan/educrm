using EduCrm.Modules.People.Domain.Entities;
using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.Modules.People.Application.Repositories;

public record PersonListItemData(
    Guid Id,
    string FullName,
    string? Email,
    string? Phone,
    int EnrolledProgramCount,
    bool HasActiveEnrollment,
    bool IsArchived);

public record PersonEnrolledProgramData(
    Guid Id,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    ProgramStatus Status);

public record PersonFollowUpData(
    string Title,
    Domain.Enums.FollowUpStatus Status,
    Domain.Enums.FollowUpType Type,
    DateTime DueAtUtc,
    DateTime? SnoozedUntilUtc,
    string? ProgramName);

public interface IPersonRepository
{
    void Add(Person person);

    Task<Person?> GetByIdAsync(Guid personId, Guid organizationId, CancellationToken ct);

    Task<Person?> GetTrackedByIdAsync(Guid personId, Guid organizationId, CancellationToken ct);

    Task<IReadOnlyList<PersonEnrolledProgramData>> GetEnrolledProgramsAsync(
        Guid personId,
        Guid organizationId,
        CancellationToken ct);

    Task<IReadOnlyList<PersonFollowUpData>> GetFollowUpsAsync(
        Guid personId,
        Guid organizationId,
        CancellationToken ct);

    Task<(IReadOnlyList<PersonListItemData> Items, int TotalCount, int EnrolledCount, int NotEnrolledCount)> GetPagedListAsync(
        Guid organizationId,
        int page,
        int pageSize,
        CancellationToken ct,
        string? searchTerm = null,
        string? preFilter = null,
        bool showArchived = false);
}