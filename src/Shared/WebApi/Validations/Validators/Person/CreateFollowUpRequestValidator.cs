using EduCrm.WebApi.Contracts.Person;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Person;

public sealed class CreateFollowUpRequestValidator : AbstractValidator<CreateFollowUpRequest>
{
    public CreateFollowUpRequestValidator()
    {
        RuleFor(x => x.PersonId)
            .NotEmpty();

        RuleFor(x => x.Type)
            .IsInEnum();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.DueAtUtc)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("DueAtUtc must be a future date.");

        RuleFor(x => x.Note)
            .MaximumLength(4000)
            .When(x => x.Note is not null);
    }
}

