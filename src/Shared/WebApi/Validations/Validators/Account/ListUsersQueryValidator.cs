using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.WebApi.Contracts.Account;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Account;

public sealed class ListUsersQueryValidator : AbstractValidator<ListUsersQuery>
{
    public ListUsersQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");

        RuleFor(x => x.Status)
            .Must(BeValidStatusList)
            .WithMessage("Status must be a comma-separated list of valid user statuses (WaitingForActivation, Active, Disabled).")
            .When(x => !string.IsNullOrWhiteSpace(x.Status));
    }

    private static bool BeValidStatusList(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return true;

        foreach (var token in value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (!Enum.TryParse<UserStatus>(token, ignoreCase: true, out _))
                return false;
        }

        return true;
    }
}
