using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.Modules.Program.Application.UseCases.Create;

public sealed record CreateInput(
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
