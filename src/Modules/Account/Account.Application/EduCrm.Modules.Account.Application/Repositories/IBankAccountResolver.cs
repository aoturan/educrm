namespace EduCrm.Modules.Account.Application.Repositories;

public interface IBankAccountResolver
{
    IReadOnlyList<BankAccountSetting> GetAll();
}

public sealed class BankAccountSetting
{
    public string BankName { get; init; } = string.Empty;
    public string AccountHolder { get; init; } = string.Empty;
    public string Iban { get; init; } = string.Empty;
    public string Note { get; init; } = string.Empty;
}