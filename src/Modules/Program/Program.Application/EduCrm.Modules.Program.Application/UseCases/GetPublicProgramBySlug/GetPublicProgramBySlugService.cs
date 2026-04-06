using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.GetPublicProgramBySlug;

public sealed class GetPublicProgramBySlugService(
    IProgramRepository programRepo) : IGetPublicProgramBySlugService
{
    public async Task<Result<GetPublicProgramBySlugResult>> GetAsync(
        GetPublicProgramBySlugInput input,
        CancellationToken ct)
    {
        var data = await programRepo.GetPublicBySlugAsync(input.Slug, ct);

        if (data is null)
            return Result<GetPublicProgramBySlugResult>.Fail(ProgramErrors.ProgramNotFound(Guid.Empty));

        return Result<GetPublicProgramBySlugResult>.Success(new GetPublicProgramBySlugResult(
            data.Name,
            data.StartDate,
            data.EndDate,
            data.Capacity,
            data.PublicShortDescription,
            data.PublicDetailedDescription,
            data.PublicModality,
            data.PublicScheduleText,
            data.PublicInstructorName,
            data.PublicEnrollmentDeadline,
            data.LocationDetails,
            data.OnlineParticipationInfo,
            data.PriceAmount,
            data.PriceCurrency,
            data.PriceNote,
            data.IsPublic,
            data.OrganizationName));
    }
}

