using EduCrm.WebApi.Contracts.Admin;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Admin;

public sealed class OverrideOrganizationSubscriptionRequestValidator
    : AbstractValidator<OverrideOrganizationSubscriptionRequest>
{
    public OverrideOrganizationSubscriptionRequestValidator()
    {
        RuleFor(x => x.PlanCode)
            .IsInEnum()
            .WithMessage("PlanCode is invalid.");

        RuleFor(x => x.StartsAtUtc)
            .NotEmpty()
            .WithMessage("StartsAtUtc is required.");

        RuleFor(x => x.EndsAtUtc)
            .NotEmpty()
            .WithMessage("EndsAtUtc is required.")
            .GreaterThan(x => x.StartsAtUtc)
            .WithMessage("EndsAtUtc must be after StartsAtUtc.");
    }
}
