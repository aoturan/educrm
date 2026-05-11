using EduCrm.Modules.Account.Application.Repositories;
using Microsoft.Extensions.Configuration;

namespace EduCrm.Modules.Account.Infrastructure.Queries;

public sealed class BankAccountResolver : IBankAccountResolver
{
    private readonly IReadOnlyList<BankAccountSetting> _accounts;

    public BankAccountResolver(IConfiguration configuration)
    {
        _accounts = configuration
            .GetSection("BankAccounts")
            .Get<List<BankAccountSetting>>() ?? new List<BankAccountSetting>();
    }

    public IReadOnlyList<BankAccountSetting> GetAll() => _accounts;
}