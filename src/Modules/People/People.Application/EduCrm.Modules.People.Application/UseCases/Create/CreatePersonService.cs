using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Contracts.Abstractions;
using EduCrm.Modules.People.Application.Errors;
using EduCrm.Modules.People.Application.Helpers;
using EduCrm.Modules.People.Application.Repositories;
using EduCrm.Modules.People.Application.UseCases.Create;
using EduCrm.Modules.People.Domain.Entities;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.Usecases.Create;

public sealed class CreatePersonService(
    IPersonRepository personRepo,
    IPlanLimitsResolver planLimitsResolver,
    IUnitOfWork uow,
    IClock clock,
    IOrgContext orgContext) : ICreatePersonService
{
    public async Task<Result<CreateResult>> CreateAsync(CreateInput input, CancellationToken ct)
    {
        if (orgContext.OrganizationId is null)
            return Result<CreateResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var organizationId = orgContext.OrganizationId.Value;

        string? normalizedPhone = null;
        if (input.Phone is not null)
        {
            var phoneResult = PhoneNormalizer.NormalizePhone(input.Phone);
            if (!phoneResult.IsSuccess)
                return Result<CreateResult>.Fail(phoneResult.Errors);
            normalizedPhone = phoneResult.Value;
        }

        var normalizedEmail = input.Email?.Trim().ToLowerInvariant();

        if (normalizedEmail is not null || normalizedPhone is not null)
        {
            var duplicate = await personRepo.ExistsByContactAsync(
                organizationId, normalizedEmail ?? string.Empty, normalizedPhone ?? string.Empty, ct);
            if (duplicate)
                return Result<CreateResult>.Fail(PeopleErrors.DuplicateContactInfo());
        }

        var limits = await planLimitsResolver.ResolveAsync(organizationId, ct);
        var currentActive = await personRepo.CountActiveByOrganizationAsync(organizationId, ct);
        if (currentActive >= limits.ActivePersons)
            return Result<CreateResult>.Fail(PeopleErrors.PlanActivePersonLimitReached(limits.ActivePersons));

        var now = clock.UtcNow.UtcDateTime;

        var person = new Person(
            organizationId,
            input.FullName.Trim(),
            input.Source,
            now,
            normalizedPhone,
            normalizedEmail,
            input.Notes?.Trim());

        personRepo.Add(person);
        await uow.SaveChangesAsync(ct);

        return Result<CreateResult>.Success(new CreateResult(person.Id));
    }
}