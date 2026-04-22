using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Contracts.Abstractions;
using EduCrm.Modules.Account.Contracts.Enums;
using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.Create;

public class CreateService(
    IProgramRepository programRepo,
    IOrganizationReader organizationReader,
    IOrganizationWriter organizationWriter,
    IUnitOfWork uow,
    IAppDbTransaction tx,
    IClock clock,
    IOrgContext orgContext,
    ICurrentUser currentUser) : ICreateService
{
    public async Task<Result<CreateResult>> CreateAsync(CreateInput input, CancellationToken ct)
    {
        if (currentUser.UserId is null)
        {
            return Result<CreateResult>.Fail(CommonErrors.Unauthorized());
        }

        if (orgContext.OrganizationId is null)
        {
            return Result<CreateResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));
        }

        var now = clock.UtcNow.UtcDateTime;
        var today = DateOnly.FromDateTime(now);

        var organization = await organizationReader.GetSubscriptionInfoAsync(orgContext.OrganizationId.Value, ct);
        if (organization is null)
        {
            return Result<CreateResult>.Fail(ProgramErrors.OrganizationNotFound(orgContext.OrganizationId.Value));
        }

        // Kural: Sadece tek 1 program free  olabilir. Bu o kontroldür.
        if (organization is { PlanType: OrganizationPlanType.Free, FreeProgramConsumedAtUtc: not null })
        {
            return Result<CreateResult>.Fail(ProgramErrors.SubscriptionRequired());
        }

        // Aktif bir subscriotion olmalı. Individual veya Academy
        if (organization.PlanType is OrganizationPlanType.Individual or OrganizationPlanType.Academy)
        {
            if (organization.SubscriptionStatus != SubscriptionStatus.Active)
            {
                return Result<CreateResult>.Fail(ProgramErrors.SubscriptionInactive());
            }
        }

        // subscription süresi bitmiş ise gg olmalı
        if (organization.SubscriptionEndsAtUtc is not null && organization.SubscriptionEndsAtUtc < now)
        {
            return Result<CreateResult>.Fail(ProgramErrors.SubscriptionExpired());
        }

        // Modality kuralları
        string? locationDetails;
        string? onlineParticipationInfo;

        if (input.PublicModality == Domain.Enums.ProgramModality.Online)
        {
            if (string.IsNullOrWhiteSpace(input.OnlineParticipationInfo))
                return Result<CreateResult>.Fail(ProgramErrors.OnlineModalityRequiresParticipationInfo());

            onlineParticipationInfo = input.OnlineParticipationInfo;
            locationDetails = null;
        }
        else
        {
            if (string.IsNullOrWhiteSpace(input.LocationDetails))
                return Result<CreateResult>.Fail(ProgramErrors.OnsiteOrHybridModalityRequiresLocationDetails());

            locationDetails = input.LocationDetails;
            onlineParticipationInfo = null;
        }

        var isPaid = input.PriceType == Domain.Enums.ProgramPriceType.Paid;

        var program = new Domain.Entities.Program(
            orgContext.OrganizationId.Value,
            currentUser.UserId.Value,
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

        await using var trx = await tx.BeginAsync(ct);

        try
        {
            if (organization.PlanType == OrganizationPlanType.Free && organization.FreeProgramConsumedAtUtc is null)
            {
                var updated = await organizationWriter.UpdateFreeProgramConsumedAtUtcAsync(orgContext.OrganizationId.Value, now, ct);
                if (!updated)
                {
                    await trx.RollbackAsync(ct);
                    return Result<CreateResult>.Fail(ProgramErrors.OrganizationNotFound(orgContext.OrganizationId.Value));
                }
            }

            programRepo.Add(program);
            await uow.SaveChangesAsync(ct);
            await trx.CommitAsync(ct);

            return Result<CreateResult>.Success(new CreateResult(program.Id));
        }
        catch
        {
            await trx.RollbackAsync(ct);
            throw;
        }
    }
}
