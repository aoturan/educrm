using EduCrm.WebApi.Contracts.Program;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Program;

public sealed class CreateEnrollmentRequestValidator : AbstractValidator<CreateEnrollmentRequest>
{
    public CreateEnrollmentRequestValidator()
    {
        RuleFor(x => x.ProgramId)
            .NotEmpty()
            .WithMessage("ProgramId is required.");

        RuleFor(x => x.PersonId)
            .NotEmpty()
            .WithMessage("PersonId is required.");
    }
}

