using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.Modules.Program.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.ChangeStatus;

public sealed class ChangeProgramStatusService(
    IProgramRepository programRepo,
    IUnitOfWork uow,
    IClock clock,
    IOrgContext orgContext) : IChangeProgramStatusService
{
    public async Task<Result<ChangeProgramStatusResult>> ChangeAsync(ChangeProgramStatusInput input, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
        {
            return Result<ChangeProgramStatusResult>.Fail(
                CommonErrors.Forbidden("Organization scope is missing."));
        }

        if (input.Status is not (ProgramStatus.Completed or ProgramStatus.Archived))
        {
            return Result<ChangeProgramStatusResult>.Fail(ProgramErrors.UnsupportedTargetStatus());
        }

        var program = await programRepo.GetTrackedByIdAsync(input.ProgramId, orgContext.OrganizationId.Value, ct);
        if (program is null)
        {
            return Result<ChangeProgramStatusResult>.Fail(ProgramErrors.ProgramNotFound(input.ProgramId));
        }

        var now = clock.UtcNow.UtcDateTime;

        try
        {
            if (input.Status == ProgramStatus.Completed)
            {
                program.Complete(now);
            }
            else
            {
                program.Archive(now);
            }
        }
        catch (InvalidOperationException ex)
        {
            return Result<ChangeProgramStatusResult>.Fail(ProgramErrors.InvalidStatusTransition(ex.Message));
        }

        await uow.SaveChangesAsync(ct);

        return Result<ChangeProgramStatusResult>.Success(
            new ChangeProgramStatusResult(program.Id, program.Status, program.CompletedAtUtc));
    }
}

