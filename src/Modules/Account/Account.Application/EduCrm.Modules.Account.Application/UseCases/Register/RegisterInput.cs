namespace EduCrm.Modules.Account.Application.UseCases.Register;

public sealed record RegisterInput(
    string Name,
    string Email,
    string OrganizationName,
    string PasswordHash,
    string Phone);
