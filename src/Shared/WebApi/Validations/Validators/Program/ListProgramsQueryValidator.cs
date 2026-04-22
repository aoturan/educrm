using EduCrm.WebApi.Contracts.Program;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Program;

public sealed class ListProgramsQueryValidator : AbstractValidator<ListProgramsQuery>
{
    public ListProgramsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");

        RuleFor(x => x.Q)
            .MaximumLength(100)
            .WithMessage("Q must not exceed 100 characters.")
            .When(x => x.Q is not null);

        RuleFor(x => x.PreFilter)
            .MaximumLength(50)
            .WithMessage("PreFilter must not exceed 50 characters.")
            .When(x => x.PreFilter is not null);

        RuleFor(x => x.Face)
            .MaximumLength(50)
            .WithMessage("Face must not exceed 50 characters.")
            .When(x => x.Face is not null);
    }
}

