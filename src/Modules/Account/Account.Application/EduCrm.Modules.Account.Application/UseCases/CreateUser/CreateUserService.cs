using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.CreateUser;

public sealed class CreateUserService(
    IUserRepository userRepo,
    IUnitOfWork uow,
    IClock clock)
    : ICreateUserService
{
    public async Task<Result<CreateUserResult>> CreateAsync(
        CreateUserInput input,
        CancellationToken ct)
    {
        var caller = await userRepo.GetByIdAsync(input.CallerUserId, ct);
        if (caller is null)
            return Result<CreateUserResult>.Fail(AccountErrors.NotFound(input.CallerUserId));

        if (caller.OrganizationId != input.CallerOrganizationId)
            return Result<CreateUserResult>.Fail(AccountErrors.UserNotInOrganization());

        if (caller.Status != UserStatus.Active)
            return Result<CreateUserResult>.Fail(AccountErrors.UserInactive());

        if (caller.Role != UserRole.Admin)
            return Result<CreateUserResult>.Fail(AccountErrors.NotAdmin());

        var normalizedEmail = input.Email.Trim().ToLowerInvariant();

        var emailTaken = await userRepo.ExistsByEmailInOrganizationAsync(normalizedEmail, caller.OrganizationId, ct);
        if (emailTaken)
            return Result<CreateUserResult>.Fail(AccountErrors.EmailTaken(normalizedEmail));

        var now = clock.UtcNow.UtcDateTime;
        var fullName = input.Name.Trim();

        var user = new User(
            Guid.NewGuid(),
            caller.OrganizationId,
            normalizedEmail,
            fullName,
            input.PasswordHash,
            UserRole.Member,
            now);

        user.Enable(now);

        userRepo.Add(user);
        await uow.SaveChangesAsync(ct);

        return Result<CreateUserResult>.Success(new CreateUserResult(
            user.Id,
            user.Email,
            user.FullName));
    }
}