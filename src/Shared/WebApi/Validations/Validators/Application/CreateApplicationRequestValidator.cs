using EduCrm.WebApi.Contracts.Application;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Application;

public sealed class CreateApplicationRequestValidator : AbstractValidator<CreateApplicationRequest>
{
    public CreateApplicationRequestValidator()
    {
        RuleFor(x => x.ProgramSlug)
            .NotEmpty()
            .WithMessage("ProgramSlug is required.");

        RuleFor(x => x.SubmittedEmail)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email is not valid.");

        RuleFor(x => x.SubmittedPhone)
            .NotEmpty()
            .WithMessage("Phone is required.");
    }
}

