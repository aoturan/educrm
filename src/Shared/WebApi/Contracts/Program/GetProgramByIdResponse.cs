using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Program;

public sealed record ProgramEnrollmentResponse(
    Guid EnrollmentId,
    Guid PersonId,
    DateTime EnrolledAtUtc,
    string FullName,
    string? Email,
    string? Phone);

public sealed record GetProgramByIdResponse(
    Guid Id,
    Guid OrganizationId,
    Guid CreatedByUserId,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    string PublicShortDescription,
    string? InternalNotes,
    string? PublicDetailedDescription,
    ProgramModality PublicModality,
    string? LocationDetails,
    string? OnlineParticipationInfo,
    int? Capacity,
    string PublicScheduleText,
    string? PublicInstructorName,
    DateOnly? PublicEnrollmentDeadline,
    bool IsPublic,
    ProgramStatus Status,
    DateTime? CompletedAtUtc,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc,
    bool IsArchived,
    DateTime? ArchivedAtUtc,
    int? PriceAmount,
    PriceCurrency? PriceCurrency,
    string? PriceNote,
    string? PublicSlug,
    DateTime? PublicPublishedAtUtc,
    IReadOnlyList<ProgramEnrollmentResponse> Enrollments);

