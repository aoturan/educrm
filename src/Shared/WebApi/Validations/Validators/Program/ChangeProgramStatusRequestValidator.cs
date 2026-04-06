using EduCrm.Modules.Program.Domain.Enums;
using EduCrm.WebApi.Contracts.Program;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Program;

public sealed class ChangeProgramStatusRequestValidator : AbstractValidator<ChangeProgramStatusRequest>
{
    public ChangeProgramStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .Must(status => status is ProgramStatus.Completed or ProgramStatus.Archived)
            .WithMessage("Status must be either Completed or Archived.");
    }
}

