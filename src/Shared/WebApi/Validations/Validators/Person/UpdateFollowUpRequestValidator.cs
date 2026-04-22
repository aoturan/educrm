using EduCrm.WebApi.Contracts.Person;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Person;

public sealed class UpdateFollowUpRequestValidator : AbstractValidator<UpdateFollowUpRequest>
{
    public UpdateFollowUpRequestValidator()
    {
        RuleFor(x => x.PersonId)
            .NotEmpty();

        RuleFor(x => x.Type)
            .IsInEnum();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Note)
            .MaximumLength(2000)
            .When(x => x.Note is not null);
    }
}

