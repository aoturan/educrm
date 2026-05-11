using EduCrm.Modules.Account.Domain.Entities;

namespace EduCrm.Modules.Account.Application.Helpers;

public static class RoleLabel
{
    public const string ApplicationAdmin = "Upphaf";

    public static string Resolve(User user) =>
        user.IsApplicationAdmin ? ApplicationAdmin : user.Role.ToString();
}