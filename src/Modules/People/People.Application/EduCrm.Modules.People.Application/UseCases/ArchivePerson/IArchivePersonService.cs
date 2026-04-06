using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.ArchivePerson;

public interface IArchivePersonService
{
    Task<Result<ArchivePersonResult>> ArchiveAsync(ArchivePersonInput input, CancellationToken ct);
}

