using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Helpers;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Application.Repositories.Models;
using EduCrm.Modules.Account.Application.Security;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.Login;

public sealed class LoginService(
    IUserRepository userRepo,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IUnitOfWork uow,
    IClock clock)
    : ILoginService
{
    public async Task<Result<LoginResult>> LoginAsync(LoginInput input, CancellationToken ct)
    {
        // 1) Find user by email (with name_surname projection)
        var userWithName = await userRepo.GetByEmailWithOrganizationAsync(input.Email, ct);
        if (userWithName is null)
        {
            return Result<LoginResult>.Fail(AccountErrors.InvalidCredentials());
        }

        // 2) Verify password
        var isPasswordValid = passwordHasher.Verify(input.Password, userWithName.PasswordHash);
        if (!isPasswordValid)
        {
            return Result<LoginResult>.Fail(AccountErrors.InvalidCredentials());
        }

        // 3) Check user status
        if (userWithName.Status == UserStatus.WaitingForActivation)
        {
            // Email not yet verified: signal the frontend to route to the verify-email screen.
            // No JWT is issued and LastLogin is not updated until activation completes.
            return Result<LoginResult>.Success(new LoginResult(
                Email: userWithName.Email,
                Status: UserStatus.WaitingForActivation,
                Token: null,
                FullName: null,
                Initials: null,
                OrganizationName: null,
                Role: null));
        }

        if (userWithName.Status != UserStatus.Active)
        {
            return Result<LoginResult>.Fail(AccountErrors.UserInactive());
        }

        // 4) Update last login timestamp
        var trackedUser = await userRepo.GetByIdAsync(userWithName.Id, ct);
        if (trackedUser is not null)
        {
            trackedUser.SetLastLogin(clock.UtcNow.UtcDateTime);
            await uow.SaveChangesAsync(ct);
        }

        // 5) Generate JWT token
        var token = jwtService.GenerateToken(userWithName.Id, userWithName.IsApplicationAdmin);

        // 6) Get initials from name
        var initials = NameHelper.GetInitials(userWithName.FullName);

        // 7) Return result
        var roleLabel = userWithName.IsApplicationAdmin
            ? RoleLabel.ApplicationAdmin
            : userWithName.Role.ToString();

        return Result<LoginResult>.Success(new LoginResult(
            Email: userWithName.Email,
            Status: userWithName.Status,
            Token: token,
            FullName: userWithName.FullName,
            Initials: initials,
            OrganizationName: userWithName.OrganizationName,
            Role: roleLabel));
    }
}
