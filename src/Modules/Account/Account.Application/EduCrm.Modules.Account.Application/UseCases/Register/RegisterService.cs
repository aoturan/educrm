using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Application.Helpers;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Application.Security;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.Register;

public sealed class RegisterService(
    IOrganizationRepository orgRepo,
    IUserRepository userRepo,
    IUnitOfWork uow,
    IAppDbTransaction tx,
    IClock clock,
    IJwtService jwtService)
    : IRegisterService
{
    public async Task<Result<RegisterResult>> RegisterAsync(
        RegisterInput input,
        CancellationToken ct)
    {
        // Check if email already exists
        var existingUser = await userRepo.GetByEmailAsync(input.Email, ct);
        if (existingUser is not null)
        {
            return Result<RegisterResult>.Fail(AccountErrors.EmailTaken(input.Email));
        }

        await using var trx = await tx.BeginAsync(ct);

        try
        {
            var now = clock.UtcNow.UtcDateTime;
            var accountId = Guid.NewGuid();

            // 1) Organization
            var organization = new Organization(
                accountId,
                input.OrganizationName,
                now);

            orgRepo.Add(organization);

            // 2) User
            var user = new User(
                Guid.NewGuid(),
                accountId,
                input.Email,
                input.Name,
                input.Phone,
                input.PasswordHash,
                now);

            userRepo.Add(user);

            // single commit
            await uow.SaveChangesAsync(ct);
            await trx.CommitAsync(ct);

            // 3) Generate JWT token
            var token = jwtService.GenerateToken(user.Id);

            var initials = NameHelper.GetInitials(input.Name);

            return Result<RegisterResult>.Success(new RegisterResult(
                token,
                input.Email,
                input.Name, 
                initials, 
                input.OrganizationName));
        }
        catch
        {
            await trx.RollbackAsync(ct);
            throw;
        }
    }
}