using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Program;

public sealed record CreateProgramRequest(
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    string PublicShortDescription,
    ProgramModality PublicModality,
    string PublicScheduleText,
    ProgramPriceType PriceType,
    string? InternalNotes = null,
    string? PublicDetailedDescription = null,
    string? LocationDetails = null,
    string? OnlineParticipationInfo = null,
    int? Capacity = null,
    string? PublicInstructorName = null,
    DateOnly? PublicEnrollmentDeadline = null,
    int? PriceAmount = null,
    PriceCurrency? PriceCurrency = null,
    string? PriceNote = null
);
