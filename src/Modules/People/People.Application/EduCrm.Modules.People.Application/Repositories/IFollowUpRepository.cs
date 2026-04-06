using EduCrm.Modules.People.Domain.Entities;
using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.Modules.People.Application.Repositories;

public record FollowUpPersonInfo(Guid Id, string FullName, string? Email, string? Phone);
public record FollowUpProgramInfo(Guid Id, string Name);

public record FollowUpByIdData(
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
    FollowUpPersonInfo Person,
    FollowUpProgramInfo? Program);

public record FollowUpListItemData(
    Guid Id,
    string PersonName,
    string? ProgramName,
    FollowUpType Type,
    FollowUpStatus Status,
    string Title,
    DateTime DueAtUtc,
    DateTime? SnoozedUntilUtc);

public interface IFollowUpRepository
{
    void Add(FollowUp followUp);

    Task<FollowUpByIdData?> GetByIdAsync(Guid followUpId, Guid organizationId, CancellationToken ct);

    Task<FollowUp?> GetTrackedByIdAsync(Guid followUpId, Guid organizationId, CancellationToken ct);

    Task<(IReadOnlyList<FollowUpListItemData> Items, int TotalCount)> GetListAsync(
        Guid organizationId,
        int page,
        int pageSize,
        CancellationToken ct,
        IReadOnlyList<FollowUpType>? typeFilter = null,
        IReadOnlyList<FollowUpStatus>? statusFilter = null,
        Guid? personId = null,
        Guid? programId = null);
}
