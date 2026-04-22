using EduCrm.WebApi.Contracts.Support;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Support;

public sealed class CreateSupportRequestRequestValidator : AbstractValidator<CreateSupportRequestRequest>
{
    public CreateSupportRequestRequestValidator()
    {
        RuleFor(x => x.Subject)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(x => x.PageUrl)
            .MaximumLength(500)
            .When(x => x.PageUrl is not null);
    }
}

