using EduCrm.WebApi.Contracts.Account;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Account;

public sealed class UpdateOrganizationRequestValidator : AbstractValidator<UpdateOrganizationRequest>
{
    public UpdateOrganizationRequestValidator()
    {
        RuleFor(x => x.OrganizationName)
            .NotEmpty().WithMessage("Organizasyon adı gereklidir.")
            .MaximumLength(200).WithMessage("Organizasyon adı en fazla 200 karakter olabilir.");

        RuleFor(x => x.ContactName)
            .NotEmpty().WithMessage("İletişim adı gereklidir.")
            .MaximumLength(200).WithMessage("İletişim adı en fazla 200 karakter olabilir.");

        RuleFor(x => x.ContactEmail)
            .NotEmpty().WithMessage("İletişim e-postası gereklidir.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
            .MaximumLength(320).WithMessage("E-posta en fazla 320 karakter olabilir.");

        RuleFor(x => x.ContactPhone)
            .NotEmpty().WithMessage("İletişim telefonu gereklidir.")
            .Matches(@"^\+?\d{10,12}$")
            .WithMessage("Lütfen geçerli bir telefon numarası giriniz. (örn: +905XXXXXXXXX)");
    }
}
