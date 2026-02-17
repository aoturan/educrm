namespace EduCrm.Modules.Account.Application.Helpers;

public static class NameHelper
{
    public static string GetInitials(string nameSurname)
    {
        if (string.IsNullOrWhiteSpace(nameSurname))
            return string.Empty;

        var parts = nameSurname
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (parts.Length == 0)
            return string.Empty;

        var initials = string.Concat(parts.Select(part => 
            part.Length > 0 ? char.ToUpper(part[0]) : '\0'));

        return initials;
    }
}

