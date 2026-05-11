using EduCrm.WebApi.Contracts.Account;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Account;

public sealed class ChangeUserPasswordByAdminRequestValidator : AbstractValidator<ChangeUserPasswordByAdminRequest>
{
    public ChangeUserPasswordByAdminRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId gereklidir.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre gereklidir.")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.");
    }
}
