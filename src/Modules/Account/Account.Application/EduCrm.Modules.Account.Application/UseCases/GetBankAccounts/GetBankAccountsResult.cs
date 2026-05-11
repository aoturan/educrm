namespace EduCrm.Modules.Account.Application.UseCases.GetBankAccounts;

public sealed record GetBankAccountsResult(IReadOnlyList<BankAccountResultItem> Items);

public sealed record BankAccountResultItem(
    string BankName,
    string AccountHolder,
    string Iban,
    string Note);