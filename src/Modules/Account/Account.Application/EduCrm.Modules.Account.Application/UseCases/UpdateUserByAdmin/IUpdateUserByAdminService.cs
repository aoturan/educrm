using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.UpdateUserByAdmin;

public interface IUpdateUserByAdminService
{
    Task<Result<UpdateUserByAdminResult>> UpdateAsync(UpdateUserByAdminInput input, CancellationToken ct);
}
