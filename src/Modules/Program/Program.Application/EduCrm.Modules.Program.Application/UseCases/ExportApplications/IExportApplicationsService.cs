using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.ExportApplications;

public interface IExportApplicationsService
{
    Task<Result<ExportApplicationsResult>> ExportAsync(ExportApplicationsInput input, CancellationToken ct);
}
