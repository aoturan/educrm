using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.TransferAdminRole;

public interface ITransferAdminRoleService
{
    Task<Result> TransferAsync(TransferAdminRoleInput input, CancellationToken ct);
}
