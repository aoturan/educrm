using EduCrm.Modules.Account.Application.Errors;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ListUsers;

public sealed class ListUsersService(
    IUserRepository userRepo,
    ICurrentUserSnapshot user)
    : IListUsersService
{
    public async Task<Result<ListUsersPagedResult>> ListAsync(ListUsersInput input, CancellationToken ct)
    {
        if (user.Role != UserRole.Admin)
            return Result<ListUsersPagedResult>.Fail(AccountErrors.NotAdmin());

        var queryResult = await userRepo.GetPagedListByOrganizationAsync(
            user.OrganizationId,
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
