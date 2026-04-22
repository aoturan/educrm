using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Program;

public sealed record PublicProgramResponse(
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
    ProgramPriceType PriceType,
    bool IsPublic,
    string OrganizationName);

