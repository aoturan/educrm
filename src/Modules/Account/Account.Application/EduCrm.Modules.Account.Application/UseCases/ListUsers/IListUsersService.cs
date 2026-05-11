using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ListUsers;

public interface IListUsersService
{
    Task<Result<ListUsersPagedResult>> ListAsync(ListUsersInput input, CancellationToken ct);
}
