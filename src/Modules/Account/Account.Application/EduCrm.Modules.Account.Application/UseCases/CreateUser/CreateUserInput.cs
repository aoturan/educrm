namespace EduCrm.Modules.Account.Application.UseCases.CreateUser;

public sealed record CreateUserInput(
    Guid CallerUserId,
    Guid CallerOrganizationId,
    string Name,
    string Email,
    string PasswordHash);