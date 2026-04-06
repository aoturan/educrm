using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.UnpublishProgram;

public sealed class UnpublishProgramService(
    IProgramRepository programRepo,
    IUnitOfWork uow,
    IClock clock,
    IOrgContext orgContext,
    ICurrentUser currentUser) : IUnpublishProgramService
{
    public async Task<Result<UnpublishProgramResult>> UnpublishAsync(UnpublishProgramInput input, CancellationToken ct)
    {
        if (currentUser.UserId is null)
            return Result<UnpublishProgramResult>.Fail(CommonErrors.Unauthorized());

        if (orgContext.OrganizationId is null)
            return Result<UnpublishProgramResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var program = await programRepo.GetTrackedByIdAsync(input.ProgramId, orgContext.OrganizationId.Value, ct);
        if (program is null)
            return Result<UnpublishProgramResult>.Fail(ProgramErrors.ProgramNotFound(input.ProgramId));

        if (!program.IsPublic)
            return Result<UnpublishProgramResult>.Fail(ProgramErrors.ProgramNotPublic(input.ProgramId));

        program.Unpublish(clock.UtcNow.UtcDateTime);

        await uow.SaveChangesAsync(ct);

        return Result<UnpublishProgramResult>.Success(new UnpublishProgramResult(program.Id));
    }
}

