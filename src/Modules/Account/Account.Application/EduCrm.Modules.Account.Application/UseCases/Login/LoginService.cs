using EduCrm.Modules.Account.Application.Helpers;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Application.Repositories.Models;
using EduCrm.Modules.Account.Application.Security;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.Login;

public sealed class LoginService(
    IUserRepository userRepo,
    IPasswordHasher passwordHasher,
    IJwtService jwtService)
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

        // 3) Check user status - only Active and WaitingForActivation can login
        if (userWithName.Status != UserStatus.Active && userWithName.Status != UserStatus.WaitingForActivation)
        {
            return Result<LoginResult>.Fail(AccountErrors.UserInactive());
        }

        // 4) Generate JWT token
        var token = jwtService.GenerateToken(userWithName.Id);

        // 5) Get initials from name
        var initials = NameHelper.GetInitials(userWithName.FullName);

        // 6) Return result
        return Result<LoginResult>.Success(new LoginResult(
            token,
            userWithName.Email,
            userWithName.FullName, 
            initials, 
            userWithName.OrganizationName));
    }
}
