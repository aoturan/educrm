using FluentValidation;
using EduCrm.WebApi.Contracts.Account;

namespace EduCrm.WebApi.Validations.Validators.Account;

public sealed class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Ad soyad gereklidir.")
            .MaximumLength(100).WithMessage("Ad soyad en fazla 100 karakter olabilir.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta gereklidir.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

        RuleFor(x => x.OrganizationName)
            .NotEmpty().WithMessage("Organizasyon adı gereklidir.")
            .MaximumLength(200).WithMessage("Organizasyon adı en fazla 200 karakter olabilir.");
    }
}
