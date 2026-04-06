using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.Modules.Program.Application.Repositories;

public record ProgramListItemData(
    Guid Id,
    string Name,
    string PublicShortDescription,
    Domain.Enums.ProgramStatus Status,
    DateOnly StartDate,
    DateOnly EndDate,
    int EnrollmentCount,
    bool IsArchived);

public record EnrollmentWithPersonData(
    Guid EnrollmentId,
    Guid PersonId,
    DateTime EnrolledAtUtc,
    string FullName,
    string? Email,
    string? Phone);

public record ProgramPagedListQueryResult(
    IReadOnlyList<ProgramListItemData> Items,
    int TotalCount);

public record PublicProgramData(
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    int? Capacity,
    string PublicShortDescription,
    string? PublicDetailedDescription,
    ProgramModality PublicModality,
    string PublicScheduleText,
    string? PublicInstructorName,
    DateOnly? PublicEnrollmentDeadline,
    string? LocationDetails,
    string? OnlineParticipationInfo,
    int? PriceAmount,
    PriceCurrency? PriceCurrency,
    string? PriceNote,
    bool IsPublic,
    string OrganizationName);

public interface IProgramRepository
{
    void Add(Domain.Entities.Program program);

    Task<IReadOnlyList<Domain.Entities.Program>> GetActiveByOrganizationIdAsync(Guid organizationId, CancellationToken ct);

    Task<IReadOnlyList<ProgramListItemData>> GetAllByOrganizationIdAsync(
        Guid organizationId,
        CancellationToken ct,
        string? nameQuery = null);

    Task<ProgramPagedListQueryResult> GetPagedListAsync(
        Guid organizationId,
        int page,
        int pageSize,
        CancellationToken ct,
        string? searchTerm = null,
        string? preFilter = null,
        bool showArchived = false,
        Guid? personId = null);

    Task<(Domain.Entities.Program program, IReadOnlyList<EnrollmentWithPersonData> enrollments)?> GetByIdAsync(
        Guid programId,
        Guid organizationId,
        CancellationToken ct);

    Task<Domain.Entities.Program?> GetTrackedByIdAsync(
        Guid programId,
        Guid organizationId,
        CancellationToken ct);

    Task<(Domain.Enums.ProgramStatus Status, DateOnly EndDate)?> GetEnrollmentCheckAsync(
        Guid programId,
        Guid organizationId,
        CancellationToken ct);

    Task<bool> ExistsAsync(Guid programId, Guid organizationId, CancellationToken ct);

    Task<PublicProgramData?> GetPublicBySlugAsync(string slug, CancellationToken ct);
}