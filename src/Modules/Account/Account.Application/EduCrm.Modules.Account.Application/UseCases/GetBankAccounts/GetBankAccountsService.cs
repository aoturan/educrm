using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Account.Application.UseCases.GetBankAccounts;

public sealed class GetBankAccountsService(IBankAccountResolver bankAccountResolver)
    : IGetBankAccountsService
{
    public Task<Result<GetBankAccountsResult>> GetAsync(CancellationToken ct)
    {
        var items = bankAccountResolver
            .GetAll()
            .Select(a => new BankAccountResultItem(a.BankName, a.AccountHolder, a.Iban, a.Note))
            .ToList();

        return Task.FromResult(Result<GetBankAccountsResult>.Success(new GetBankAccountsResult(items)));
    }
}