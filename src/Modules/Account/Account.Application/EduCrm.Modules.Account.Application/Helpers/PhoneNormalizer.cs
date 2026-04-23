namespace EduCrm.Modules.Account.Application.Helpers;

public static class PhoneNormalizer
{
    public static string? Normalize(string input)
    {
        var digits = new string(input.Where(char.IsDigit).ToArray());

        if (digits.Length == 10 && digits.StartsWith("5"))
            return $"+90{digits}";

        if (digits.Length == 11 && digits.StartsWith("0"))
            return $"+90{digits.Substring(1)}";

        if (digits.Length == 12 && digits.StartsWith("90"))
            return $"+{digits}";

        return null;
    }
}