namespace EduCrm.Modules.Account.Application.Security;

public interface IJwtService
{
    string GenerateToken(Guid userId);
}
