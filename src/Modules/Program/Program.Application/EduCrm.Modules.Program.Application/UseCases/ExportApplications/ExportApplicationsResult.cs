using EduCrm.Modules.Program.Application.Repositories;

namespace EduCrm.Modules.Program.Application.UseCases.ExportApplications;

public sealed record ExportApplicationsResult(
    IReadOnlyList<ApplicationExportItem> Rows,
    DateTime GeneratedAtUtc);
