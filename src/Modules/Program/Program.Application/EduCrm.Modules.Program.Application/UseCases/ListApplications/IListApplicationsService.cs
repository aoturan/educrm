using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.ListApplications;

public interface IListApplicationsService
{
    Task<Result<ListApplicationsResult>> ListAsync(ListApplicationsInput input, CancellationToken ct);
}

