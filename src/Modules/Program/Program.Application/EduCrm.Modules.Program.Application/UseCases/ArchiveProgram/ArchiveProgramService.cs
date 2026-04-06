using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.ArchiveProgram;

public sealed class ArchiveProgramService(
    IProgramRepository programRepo,
    IUnitOfWork uow,
    IClock clock,
    IOrgContext orgContext,
    ICurrentUser currentUser) : IArchiveProgramService
{
    public async Task<Result<ArchiveProgramResult>> ArchiveAsync(ArchiveProgramInput input, CancellationToken ct)
    {
        if (currentUser.UserId is null)
            return Result<ArchiveProgramResult>.Fail(CommonErrors.Unauthorized());

        if (orgContext.OrganizationId is null)
            return Result<ArchiveProgramResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var program = await programRepo.GetTrackedByIdAsync(input.ProgramId, orgContext.OrganizationId.Value, ct);
        if (program is null)
            return Result<ArchiveProgramResult>.Fail(ProgramErrors.ProgramNotFound(input.ProgramId));

        var now = clock.UtcNow.UtcDateTime;

        if (input.ShouldArchive)
        {
            if (program.IsArchived)
                return Result<ArchiveProgramResult>.Fail(ProgramErrors.ProgramAlreadyArchived(input.ProgramId));

            program.Archive(now);
        }
        else
        {
            if (!program.IsArchived)
                return Result<ArchiveProgramResult>.Fail(ProgramErrors.ProgramNotArchived(input.ProgramId));

            program.Unarchive(now);
        }

        await uow.SaveChangesAsync(ct);

        return Result<ArchiveProgramResult>.Success(new ArchiveProgramResult(
            program.Id,
            program.IsArchived,
            program.ArchivedAtUtc,
            program.Status));
    }
}

