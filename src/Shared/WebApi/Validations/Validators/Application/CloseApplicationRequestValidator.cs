using EduCrm.WebApi.Contracts.Application;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Application;

public sealed class CloseApplicationRequestValidator : AbstractValidator<CloseApplicationRequest>
{
    public CloseApplicationRequestValidator()
    {
        RuleFor(x => x.ClosedNote)
            .MaximumLength(500)
            .WithMessage("Closed note cannot exceed 500 characters.")
            .When(x => x.ClosedNote is not null);
    }
}

