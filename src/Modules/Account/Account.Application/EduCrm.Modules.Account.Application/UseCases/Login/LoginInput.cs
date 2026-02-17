namespace EduCrm.Modules.Account.Application.UseCases.Login;

public sealed record LoginInput(
    string Email,
    string Password);
