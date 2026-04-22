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
        string? locationDetails;
        string? onlineParticipationInfo;

        if (input.PublicModality == Domain.Enums.ProgramModality.Online)
        {
            if (string.IsNullOrWhiteSpace(input.OnlineParticipationInfo))
                return Result<UpdateProgramResult>.Fail(ProgramErrors.OnlineModalityRequiresParticipationInfo());

            onlineParticipationInfo = input.OnlineParticipationInfo;
            locationDetails = null;
        }
        else
        {
            if (string.IsNullOrWhiteSpace(input.LocationDetails))
                return Result<UpdateProgramResult>.Fail(ProgramErrors.OnsiteOrHybridModalityRequiresLocationDetails());

            locationDetails = input.LocationDetails;
            onlineParticipationInfo = null;
        }

        var now = clock.UtcNow.UtcDateTime;
        var isPaid = input.PriceType == Domain.Enums.ProgramPriceType.Paid;

        program.Update(
            input.Name,
            input.StartDate,
            input.EndDate,
            input.PublicShortDescription,
            input.PublicModality,
            input.PublicScheduleText,
            now,
            input.PriceType,
            input.InternalNotes,
            input.PublicDetailedDescription,
            locationDetails,
            onlineParticipationInfo,
            input.Capacity,
            input.PublicInstructorName,
            input.PublicEnrollmentDeadline,
            isPaid ? input.PriceAmount : null,
            isPaid ? input.PriceCurrency : null,
            input.PriceNote);

        await uow.SaveChangesAsync(ct);

        return Result<UpdateProgramResult>.Success(new UpdateProgramResult(program.Id));
    }
}

