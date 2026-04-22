using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.GetById;

public sealed class GetProgramByIdService(
    IProgramRepository programRepo,
    IOrgContext orgContext) : IGetProgramByIdService
{
    public async Task<Result<GetProgramByIdResult>> GetAsync(Guid programId, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
        {
            return Result<GetProgramByIdResult>.Fail(
                CommonErrors.Forbidden("Organization scope is missing."));
        }

        var data = await programRepo.GetByIdAsync(programId, orgContext.OrganizationId.Value, ct);

        if (data is null)
        {
            return Result<GetProgramByIdResult>.Fail(ProgramErrors.OrganizationNotFound(programId));
        }

        var (program, enrollments) = data.Value;

        var result = new GetProgramByIdResult(
            program.Id,
            program.OrganizationId,
            program.CreatedByUserId,
            program.Name,
            program.StartDate,
            program.EndDate,
            program.PublicShortDescription,
            program.InternalNotes,
            program.PublicDetailedDescription,
            program.PublicModality,
            program.LocationDetails,
            program.OnlineParticipationInfo,
            program.Capacity,
            program.PublicScheduleText,
            program.PublicInstructorName,
            program.PublicEnrollmentDeadline,
            program.IsPublic,
            program.Status,
            program.CompletedAtUtc,
            program.CreatedAtUtc,
            program.UpdatedAtUtc,
            program.IsArchived,
            program.ArchivedAtUtc,
            program.PriceAmount,
            program.PriceCurrency,
            program.PriceNote,
            program.PriceType,
            program.PublicSlug,
            program.PublicPublishedAtUtc,
            enrollments
                .Select(e => new ProgramEnrollmentResult(
                    e.EnrollmentId,
                    e.PersonId,
                    e.EnrolledAtUtc,
                    e.FullName,
                    e.Email,
                    e.Phone))
                .ToList());

        return Result<GetProgramByIdResult>.Success(result);
    }
}

