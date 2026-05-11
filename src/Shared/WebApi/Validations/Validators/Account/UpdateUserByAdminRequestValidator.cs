using EduCrm.WebApi.Contracts.Account;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Account;

public sealed class UpdateUserByAdminRequestValidator : AbstractValidator<UpdateUserByAdminRequest>
{
    public UpdateUserByAdminRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId gereklidir.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Ad soyad gereklidir.")
            .MaximumLength(100).WithMessage("Ad soyad en fazla 100 karakter olabilir.");
    }
}
