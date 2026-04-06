using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.UpdateProgram;

public class UpdateProgramService(
    IProgramRepository programRepo,
    IUnitOfWork uow,
    IClock clock,
    IOrgContext orgContext,
    ICurrentUser currentUser) : IUpdateProgramService
{
    public async Task<Result<UpdateProgramResult>> UpdateAsync(UpdateProgramInput input, CancellationToken ct)
    {
        if (currentUser.UserId is null)
            return Result<UpdateProgramResult>.Fail(CommonErrors.Unauthorized());

        if (orgContext.OrganizationId is null)
            return Result<UpdateProgramResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var program = await programRepo.GetTrackedByIdAsync(input.ProgramId, orgContext.OrganizationId.Value, ct);
        if (program is null)
            return Result<UpdateProgramResult>.Fail(ProgramErrors.ProgramNotFound(input.ProgramId));

        // Modality rules
        if (input.PublicModality == Domain.Enums.ProgramModality.Online)
        {
            if (string.IsNullOrWhiteSpace(input.OnlineParticipationInfo))
                return Result<UpdateProgramResult>.Fail(ProgramErrors.OnlineModalityRequiresParticipationInfo());

            if (input.LocationDetails is not null)
                return Result<UpdateProgramResult>.Fail(ProgramErrors.OnlineModalityMustNotHaveLocationDetails());
        }
        else
        {
            if (string.IsNullOrWhiteSpace(input.LocationDetails))
                return Result<UpdateProgramResult>.Fail(ProgramErrors.OnsiteOrHybridModalityRequiresLocationDetails());

            if (input.OnlineParticipationInfo is not null)
                return Result<UpdateProgramResult>.Fail(ProgramErrors.OnsiteOrHybridModalityMustNotHaveParticipationInfo());
        }

        var now = clock.UtcNow.UtcDateTime;

        program.Update(
            input.Name,
            input.StartDate,
            input.EndDate,
            input.PublicShortDescription,
            input.PublicModality,
            input.PublicScheduleText,
            now,
            input.InternalNotes,
            input.PublicDetailedDescription,
            input.LocationDetails,
            input.OnlineParticipationInfo,
            input.Capacity,
            input.PublicInstructorName,
            input.PublicEnrollmentDeadline,
            input.PriceAmount,
            input.PriceAmount is not null ? input.PriceCurrency : null,
            input.PriceAmount is not null ? input.PriceNote : null);

        await uow.SaveChangesAsync(ct);

        return Result<UpdateProgramResult>.Success(new UpdateProgramResult(program.Id));
    }
}

