using FluentValidation;
using EduCrm.WebApi.Contracts.Account;

namespace EduCrm.WebApi.Validations.Validators.Account;

public sealed class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Mevcut şifre gereklidir.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Yeni şifre gereklidir.")
            .MinimumLength(8).WithMessage("Yeni şifre en az 8 karakter olmalıdır.")
            .NotEqual(x => x.OldPassword).WithMessage("Yeni şifre mevcut şifreden farklı olmalıdır.");
    }
}

