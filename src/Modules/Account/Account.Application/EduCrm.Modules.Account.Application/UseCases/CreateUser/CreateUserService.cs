using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Contracts.Abstractions;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.CreateUser;

public sealed class CreateUserService(
    IUserRepository userRepo,
    IPlanLimitsResolver planLimitsResolver,
    IUnitOfWork uow,
    IClock clock,
    ICurrentUserSnapshot user)
    : ICreateUserService
{
    public async Task<Result<CreateUserResult>> CreateAsync(
        CreateUserInput input,
        CancellationToken ct)
    {
        if (user.Role != UserRole.Admin)
            return Result<CreateUserResult>.Fail(AccountErrors.NotAdmin());

        var normalizedEmail = input.Email.Trim().ToLowerInvariant();

        var emailTaken = await userRepo.ExistsByEmailInOrganizationAsync(normalizedEmail, user.OrganizationId, ct);
        if (emailTaken)
            return Result<CreateUserResult>.Fail(AccountErrors.EmailTaken(normalizedEmail));

        var limits = await planLimitsResolver.ResolveAsync(user.OrganizationId, ct);
        var currentActive = await userRepo.CountActiveByOrganizationAsync(user.OrganizationId, ct);
        if (currentActive >= limits.Users)
            return Result<CreateUserResult>.Fail(AccountErrors.PlanUserLimitReached(limits.Users));

        var now = clock.UtcNow.UtcDateTime;
        var fullName = input.Name.Trim();

        var newUser = new User(
            Guid.NewGuid(),
            user.OrganizationId,
            normalizedEmail,
            fullName,
            input.PasswordHash,
            UserRole.Member,
            now);

        newUser.Enable(now);

        userRepo.Add(newUser);
        await uow.SaveChangesAsync(ct);

        return Result<CreateUserResult>.Success(new CreateUserResult(
            newUser.Id,
            newUser.Email,
            newUser.FullName,
            newUser.Role,
            newUser.Status,
            newUser.LastLoginAtUtc));
    }
}
