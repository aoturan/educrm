using ApplicationEntity = EduCrm.Modules.Program.Domain.Entities.Application;
using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Program.Application.Errors;
using EduCrm.Modules.Program.Application.Helpers;
using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.Modules.Program.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.CreateApplication;

public sealed class CreateApplicationService(
    IApplicationRepository applicationRepo,
    IProgramRepository programRepo,
    IUnitOfWork uow,
    IAppDbTransaction tx,
    IClock clock) : ICreateApplicationService
{
    public async Task<Result<CreateApplicationResult>> CreateAsync(
        CreateApplicationInput input,
        CancellationToken ct)
    {
        var programCheck = await programRepo.GetPublicApplicationCheckBySlugAsync(input.ProgramSlug, ct);

        if (programCheck is null)
            return Result<CreateApplicationResult>.Fail(ProgramErrors.ProgramNotAvailable());

        if (programCheck.Status != ProgramStatus.Active)
            return Result<CreateApplicationResult>.Fail(ProgramErrors.ProgramNotAvailable());

        if (!programCheck.IsPublic)
            return Result<CreateApplicationResult>.Fail(ProgramErrors.ProgramNotAvailable());

        var programId = programCheck.ProgramId;
        var organizationId = programCheck.OrganizationId;

        var normalizedEmail = input.SubmittedEmail.Trim().ToLowerInvariant();
        var phoneResult = PhoneNormalizer.NormalizePhone(input.SubmittedPhone);
        if (!phoneResult.IsSuccess)
            return Result<CreateApplicationResult>.Fail(phoneResult.Errors);
        var normalizedPhone = phoneResult.Value!;

        await using var trx = await tx.BeginAsync(ct);

        try
        {
            var existingApplications = await applicationRepo.GetActiveApplicationsByContactAsync(
                programId, organizationId, normalizedEmail, normalizedPhone, ct);

            if (existingApplications.Count > 1)
            {
                await trx.RollbackAsync(ct);
                return Result<CreateApplicationResult>.Fail(ProgramErrors.ApplicationAlreadyExists());
            }

            if (existingApplications.Count == 1)
            {
                existingApplications[0].IncrementSubmission(clock.UtcNow.UtcDateTime);
                await uow.SaveChangesAsync(ct);
                await trx.CommitAsync(ct);
                return Result<CreateApplicationResult>.Success(new CreateApplicationResult(existingApplications[0].Id));
            }

            var application = new ApplicationEntity(
                organizationId,
                programId,
                personId: null,
                clock.UtcNow.UtcDateTime,
                input.SubmittedFullName?.Trim() ?? normalizedEmail,
                normalizedPhone,
                normalizedEmail,
                input.SubmittedMessage ?? string.Empty);

            applicationRepo.Add(application);
            await uow.SaveChangesAsync(ct);
            await trx.CommitAsync(ct);

            return Result<CreateApplicationResult>.Success(new CreateApplicationResult(application.Id));
        }
        catch
        {
            await trx.RollbackAsync(ct);
            throw;
        }
    }
}
