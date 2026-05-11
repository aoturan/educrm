namespace EduCrm.WebApi.Contracts.Account;

public sealed record BankAccountsResponse(IReadOnlyList<BankAccountItemResponse> Items);

public sealed record BankAccountItemResponse(
    string BankName,
    string AccountHolder,
    string Iban,
    string Note);