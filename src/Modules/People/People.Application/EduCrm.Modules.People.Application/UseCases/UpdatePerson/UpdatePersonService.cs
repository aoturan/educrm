using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.People.Application.Errors;
using EduCrm.Modules.People.Application.Helpers;
using EduCrm.Modules.People.Application.Repositories;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.UpdatePerson;

public sealed class UpdatePersonService(
    IPersonRepository personRepo,
    IUnitOfWork uow,
    IClock clock,
    IOrgContext orgContext,
    ICurrentUser currentUser) : IUpdatePersonService
{
    public async Task<Result<UpdatePersonResult>> UpdateAsync(UpdatePersonInput input, CancellationToken ct)
    {
        if (currentUser.UserId is null)
            return Result<UpdatePersonResult>.Fail(CommonErrors.Unauthorized());

        if (orgContext.OrganizationId is null)
            return Result<UpdatePersonResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var organizationId = orgContext.OrganizationId.Value;

        var person = await personRepo.GetTrackedByIdAsync(input.PersonId, organizationId, ct);
        if (person is null)
            return Result<UpdatePersonResult>.Fail(PeopleErrors.PersonNotFound(input.PersonId));

        if (person.IsArchived)
            return Result<UpdatePersonResult>.Fail(PeopleErrors.PersonCannotBeUpdated(input.PersonId));

        // Normalize phone
        string? normalizedPhone = null;
        if (!string.IsNullOrWhiteSpace(input.Phone))
        {
            var phoneResult = PhoneNormalizer.NormalizePhone(input.Phone);
            if (!phoneResult.IsSuccess)
                return Result<UpdatePersonResult>.Fail(phoneResult.Errors);
            normalizedPhone = phoneResult.Value;
        }

        // Normalize email
        var normalizedEmail = string.IsNullOrWhiteSpace(input.Email)
            ? null
            : input.Email.Trim().ToLowerInvariant();

        // At least one contact field must be provided
        if (normalizedPhone is null && normalizedEmail is null)
            return Result<UpdatePersonResult>.Fail(PeopleErrors.ContactInfoRequired());

        // Detect changes
        var phoneChanged = normalizedPhone != person.Phone;
        var emailChanged = normalizedEmail != person.Email;
        var fullNameChanged = input.FullName.Trim() != person.FullName;
        var notesChanged = (input.Notes?.Trim() ?? string.Empty) != (person.Notes ?? string.Empty);

        // No changes — return success without touching the database
        if (!phoneChanged && !emailChanged && !fullNameChanged && !notesChanged)
            return Result<UpdatePersonResult>.Success(new UpdatePersonResult(person.Id));

        // Duplicate contact check if phone or email changed
        if (phoneChanged || emailChanged)
        {
            var duplicate = await personRepo.ExistsByContactExcludingAsync(
                organizationId, normalizedEmail ?? string.Empty, normalizedPhone ?? string.Empty, person.Id, ct);

            if (duplicate)
                return Result<UpdatePersonResult>.Fail(PeopleErrors.DuplicateContactInfo());
        }

        person.Update(input.FullName, normalizedPhone, normalizedEmail, input.Notes, clock.UtcNow.UtcDateTime);
        await uow.SaveChangesAsync(ct);

        return Result<UpdatePersonResult>.Success(new UpdatePersonResult(person.Id));
    }
}

