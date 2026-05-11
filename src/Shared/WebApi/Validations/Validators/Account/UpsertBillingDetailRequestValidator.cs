using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.WebApi.Contracts.Account;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Account;

public sealed class UpsertBillingDetailRequestValidator : AbstractValidator<UpsertBillingDetailRequest>
{
    public UpsertBillingDetailRequestValidator()
    {
        RuleFor(x => x.BillingType)
            .IsInEnum().WithMessage("Geçerli bir fatura türü belirtiniz (Individual veya Corporate).");

        RuleFor(x => x.BillingName)
            .NotEmpty().WithMessage("Fatura ünvanı gereklidir.")
            .MaximumLength(200).WithMessage("Fatura ünvanı en fazla 200 karakter olabilir.");

        RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage("Vergi numarası gereklidir.")
            .MaximumLength(20).WithMessage("Vergi numarası en fazla 20 karakter olabilir.");

        RuleFor(x => x.TaxOffice)
            .NotEmpty().WithMessage("Kurumsal fatura için vergi dairesi gereklidir.")
            .MaximumLength(100).WithMessage("Vergi dairesi en fazla 100 karakter olabilir.")
            .When(x => x.BillingType == BillingType.Corporate);

        RuleFor(x => x.BillingEmail)
            .NotEmpty().WithMessage("Fatura e-postası gereklidir.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
            .MaximumLength(320).WithMessage("Fatura e-postası en fazla 320 karakter olabilir.");

        RuleFor(x => x.BillingAddress)
            .NotEmpty().WithMessage("Fatura adresi gereklidir.")
            .MaximumLength(500).WithMessage("Fatura adresi en fazla 500 karakter olabilir.");
    }
}