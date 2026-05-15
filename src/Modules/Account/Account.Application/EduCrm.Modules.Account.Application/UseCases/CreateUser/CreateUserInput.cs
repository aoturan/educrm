namespace EduCrm.Modules.Account.Application.UseCases.CreateUser;

public sealed record CreateUserInput(
    string Name,
    string Email,
    string PasswordHash);
