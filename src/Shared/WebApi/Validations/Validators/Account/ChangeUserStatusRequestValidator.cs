using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.WebApi.Contracts.Account;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Account;

public sealed class ChangeUserStatusRequestValidator : AbstractValidator<ChangeUserStatusRequest>
{
    public ChangeUserStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .Must(s => s == UserStatus.Active || s == UserStatus.Disabled)
            .WithMessage("Geçerli bir durum belirtiniz (Active veya Disabled).");
    }
}