using EduCrm.WebApi.Contracts.Account;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Account;

public sealed class TransferAdminRoleRequestValidator : AbstractValidator<TransferAdminRoleRequest>
{
    public TransferAdminRoleRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId gereklidir.");
    }
}
