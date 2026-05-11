using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.ExportPersons;

public interface IExportPersonsService
{
    Task<Result<ExportPersonsResult>> ExportAsync(ExportPersonsInput input, CancellationToken ct);
}
