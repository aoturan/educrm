using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.Modules.Program.Application.UseCases.ExportApplications;

public sealed record ExportApplicationsInput(
    IReadOnlyList<ApplicationStatus>? Statuses = null,
    Guid? ProgramId = null);
