using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.Modules.Program.Application.UseCases.ListApplications;

public sealed record ListApplicationsInput(
    int Page = 1,
    int PageSize = 20,
    IReadOnlyList<ApplicationStatus>? Statuses = null,
    Guid? ProgramId = null,
    bool IsBrief = false);
