using FluentValidation;
using EduCrm.WebApi.Contracts.Account;

namespace EduCrm.WebApi.Validations.Validators.Account;

public sealed class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Ad soyad gereklidir.")
            .MaximumLength(100).WithMessage("Ad soyad en fazla 100 karakter olabilir.");
    }
}