using EduCrm.WebApi.Contracts.Person;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Person;

public sealed class RescheduleDueDateRequestValidator : AbstractValidator<RescheduleDueDateRequest>
{
    public RescheduleDueDateRequestValidator()
    {
        RuleFor(x => x.DueAtUtc)
            .NotEmpty()
            .WithMessage("DueAtUtc is required.");
    }
}

