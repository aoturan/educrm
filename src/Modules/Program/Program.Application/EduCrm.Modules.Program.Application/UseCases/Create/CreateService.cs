using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Contracts.Abstractions;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.Create;

public class CreateService(
    IProgramRepository programRepo,
    IPlanLimitsResolver planLimitsResolver,
    IUnitOfWork uow,
    IClock clock,
    ICurrentUserSnapshot user) : ICreateService
{
    public async Task<Result<CreateResult>> CreateAsync(CreateInput input, CancellationToken ct)
    {
        var now = clock.UtcNow.UtcDateTime;
        var today = DateOnly.FromDateTime(now);

        // Modality kuralları
        string? locationDetails;
        string? onlineParticipationInfo;

        if (input.PublicModality == Domain.Enums.ProgramModality.Online)
        {
            if (string.IsNullOrWhiteSpace(input.OnlineParticipationInfo))
                return Result<CreateResult>.Fail(Errors.ProgramErrors.OnlineModalityRequiresParticipationInfo());

            onlineParticipationInfo = input.OnlineParticipationInfo;
            locationDetails = null;
        }
        else
        {
            if (string.IsNullOrWhiteSpace(input.LocationDetails))
                return Result<CreateResult>.Fail(Errors.ProgramErrors.OnsiteOrHybridModalityRequiresLocationDetails());

            locationDetails = input.LocationDetails;
            onlineParticipationInfo = null;
        }

        var limits = await planLimitsResolver.ResolveAsync(user.OrganizationId, ct);
        if (limits.ActivePrograms is int cap)
        {
            var currentActive = await programRepo.CountActiveByOrganizationAsync(user.OrganizationId, ct);
            if (currentActive >= cap)
                return Result<CreateResult>.Fail(Errors.ProgramErrors.PlanActiveProgramLimitReached(cap));
        }

        var isPaid = input.PriceType == Domain.Enums.ProgramPriceType.Paid;

        var program = new Domain.Entities.Program(
            user.OrganizationId,
            user.UserId,
            input.Name,
            input.StartDate,
            input.EndDate,
            input.PublicShortDescription,
            input.PublicModality,
            input.PublicScheduleText,
            today,
            now,
            input.InternalNotes,
            input.PublicDetailedDescription,
            locationDetails,
            onlineParticipationInfo,
            input.Capacity,
            input.PublicInstructorName,
            input.PublicEnrollmentDeadline,
            isPaid ? input.PriceAmount : null,
            isPaid ? input.PriceCurrency : null,
            input.PriceNote,
            input.PriceType);

        programRepo.Add(program);
        await uow.SaveChangesAsync(ct);

        return Result<CreateResult>.Success(new CreateResult(program.Id));
    }
}