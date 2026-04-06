using EduCrm.WebApi.Contracts.Person;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Person;

public sealed class SnoozeFollowUpRequestValidator : AbstractValidator<SnoozeFollowUpRequest>
{
    public SnoozeFollowUpRequestValidator()
    {
        RuleFor(x => x.SnoozeUntilUtc)
            .NotEmpty()
            .WithMessage("SnoozeUntilUtc is required.");
    }
}

