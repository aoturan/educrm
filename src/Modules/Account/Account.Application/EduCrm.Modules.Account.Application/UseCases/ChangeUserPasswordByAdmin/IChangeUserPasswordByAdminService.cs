using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ChangeUserPasswordByAdmin;

public interface IChangeUserPasswordByAdminService
{
    Task<Result> ChangeAsync(ChangeUserPasswordByAdminInput input, CancellationToken ct);
}
