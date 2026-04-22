using EduCrm.Modules.Program.Application.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.Helpers;

public static class PhoneNormalizer
{
    public static Result<string> NormalizePhone(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return Result<string>.Fail(ProgramErrors.InvalidPhoneFormat());

        var digits = new string(input.Where(char.IsDigit).ToArray());

        if (digits.Length == 10 && digits.StartsWith("5"))
            return Result<string>.Success($"+90{digits}");

        if (digits.Length == 11 && digits.StartsWith("0"))
            return Result<string>.Success($"+90{digits.Substring(1)}");

        if (digits.Length == 12 && digits.StartsWith("90"))
            return Result<string>.Success($"+{digits}");

        return Result<string>.Fail(ProgramErrors.InvalidPhoneFormat());
    }
}
