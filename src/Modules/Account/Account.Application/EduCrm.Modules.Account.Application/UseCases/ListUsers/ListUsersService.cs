using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ListUsers;

public sealed class ListUsersService(
    IUserRepository userRepo)
    : IListUsersService
{
    public async Task<Result<ListUsersPagedResult>> ListAsync(ListUsersInput input, CancellationToken ct)
    {
        var caller = await userRepo.GetByIdAsync(input.CallerUserId, ct);
        if (caller is null)
            return Result<ListUsersPagedResult>.Fail(AccountErrors.NotFound(input.CallerUserId));

        if (caller.OrganizationId != input.CallerOrganizationId)
            return Result<ListUsersPagedResult>.Fail(AccountErrors.UserNotInOrganization());

        if (caller.Status != UserStatus.Active)
            return Result<ListUsersPagedResult>.Fail(AccountErrors.UserInactive());

        if (caller.Role != UserRole.Admin)
            return Result<ListUsersPagedResult>.Fail(AccountErrors.NotAdmin());

        var queryResult = await userRepo.GetPagedListByOrganizationAsync(
            input.CallerOrganizationId,
            input.Page,
            input.PageSize,
            ct,
            input.Statuses);

        return Result<ListUsersPagedResult>.Success(new ListUsersPagedResult(
            queryResult.Items.Select(x => new ListUsersItemResult(
                x.UserId,
                x.FullName,
                x.Email,
                x.Role,
                x.Status,
                x.LastLoginAtUtc)).ToList(),
            input.Page,
            input.PageSize,
            queryResult.TotalCount));
    }
}
