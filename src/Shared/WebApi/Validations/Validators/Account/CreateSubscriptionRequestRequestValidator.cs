using EduCrm.WebApi.Contracts.Account;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Account;

public sealed class CreateSubscriptionRequestRequestValidator : AbstractValidator<CreateSubscriptionRequestRequest>
{
    public CreateSubscriptionRequestRequestValidator()
    {
        RuleFor(x => x.RequestedPlanCode)
            .IsInEnum().WithMessage("Geçerli bir plan seçiniz.");
    }
}