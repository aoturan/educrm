namespace EduCrm.Modules.Account.Application.UseCases.CreateUser;

public sealed record CreateUserResult(Guid UserId, string Email, string FullName);