using EduCrm.WebApi.Contracts.Person;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Person;

public sealed class CreatePersonRequestValidator : AbstractValidator<CreatePersonRequest>
{
    public CreatePersonRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.Phone) || !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Telefon numarası veya e-posta adresinden en az biri girilmelidir.")
            .WithName("Contact");

        RuleFor(x => x.Phone)
            .Matches(@"^\+?\d{10,12}$")
            .WithMessage("Lütfen geçerli bir telefon numarası giriniz. (örn: +905XXXXXXXXX)")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(320)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Notes)
            .MaximumLength(2000)
            .When(x => x.Notes is not null);
    }
}
