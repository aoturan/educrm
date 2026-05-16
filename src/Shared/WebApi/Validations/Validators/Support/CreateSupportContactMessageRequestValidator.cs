using EduCrm.WebApi.Contracts.Support;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Support;

public sealed class CreateSupportContactMessageRequestValidator : AbstractValidator<CreateSupportContactMessageRequest>
{
    public CreateSupportContactMessageRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(200);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(320);

        RuleFor(x => x.Subject)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(200);

        RuleFor(x => x.Message)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(3000);

        RuleFor(x => x.TurnstileToken)
            .NotEmpty();
    }
}
