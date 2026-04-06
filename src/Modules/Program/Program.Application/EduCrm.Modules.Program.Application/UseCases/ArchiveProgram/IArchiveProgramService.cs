using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.ArchiveProgram;

public interface IArchiveProgramService
{
    Task<Result<ArchiveProgramResult>> ArchiveAsync(ArchiveProgramInput input, CancellationToken ct);
}

