namespace EduCrm.Modules.Program.Application.UseCases.GetEnrollmentCandidates;

public sealed record GetEnrollmentCandidatesInput(
    Guid ProgramId,
    string? Search,
    int Page,
    int PageSize);

