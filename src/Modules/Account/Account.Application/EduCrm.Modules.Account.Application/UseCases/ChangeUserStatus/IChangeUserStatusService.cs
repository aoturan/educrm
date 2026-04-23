using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.ChangeUserStatus;

public interface IChangeUserStatusService
{
    Task<Result> ChangeAsync(ChangeUserStatusInput input, CancellationToken ct);
}