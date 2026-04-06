using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.Modules.Program.Application.UseCases.GetPublicProgramBySlug;

public sealed record GetPublicProgramBySlugResult(
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

