using EduCrm.WebApi.Contracts.Admin;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Admin;

public sealed class ListSupportRequestsQueryValidator : AbstractValidator<ListSupportRequestsQuery>
{
    public ListSupportRequestsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");

        RuleFor(x => x.PreFilter)
            .MaximumLength(50)
            .WithMessage("PreFilter must not exceed 50 characters.")
            .When(x => x.PreFilter is not null);
    }
}
