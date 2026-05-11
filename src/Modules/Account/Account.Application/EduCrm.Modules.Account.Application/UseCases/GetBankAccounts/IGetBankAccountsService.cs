using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetBankAccounts;

public interface IGetBankAccountsService
{
    Task<Result<GetBankAccountsResult>> GetAsync(CancellationToken ct);
}