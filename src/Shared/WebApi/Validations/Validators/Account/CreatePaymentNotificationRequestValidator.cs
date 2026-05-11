using EduCrm.WebApi.Contracts.Account;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace EduCrm.WebApi.Validations.Validators.Account;

public sealed class CreatePaymentNotificationRequestValidator : AbstractValidator<CreatePaymentNotificationRequest>
{
    private const long MaxFileSizeBytes = 5 * 1024 * 1024;
    private const int MaxFileNameLength = 255;

    private static readonly HashSet<string> AllowedExtensions =
        new(StringComparer.OrdinalIgnoreCase) { ".pdf", ".jpg", ".jpeg", ".png" };

    private static readonly HashSet<string> AllowedContentTypes =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "application/pdf",
            "image/jpeg",
            "image/jpg",
            "image/png"
        };

    public CreatePaymentNotificationRequestValidator()
    {
        RuleFor(x => x.SenderName)
            .NotEmpty().WithMessage("Gönderen adı gereklidir.")
            .MaximumLength(200).WithMessage("Gönderen adı en fazla 200 karakter olabilir.");

        RuleFor(x => x.PaymentDate)
            .NotEmpty().WithMessage("Ödeme tarihi gereklidir.");

        RuleFor(x => x.Amount)
            .GreaterThan(0m).WithMessage("Tutar sıfırdan büyük olmalıdır.");

        RuleFor(x => x.Note)
            .MaximumLength(500).WithMessage("Not en fazla 500 karakter olabilir.");

        RuleFor(x => x.Receipt)
            .NotNull().WithMessage("Dekont dosyası gereklidir.");

        When(x => x.Receipt is not null, () =>
        {
            RuleFor(x => x.Receipt!.Length)
                .GreaterThan(0).WithMessage("Dekont dosyası boş olamaz.")
                .LessThanOrEqualTo(MaxFileSizeBytes)
                .WithMessage("Dekont dosyası en fazla 5 MB olabilir.");

            RuleFor(x => x.Receipt!.FileName)
                .NotEmpty().WithMessage("Dosya adı gereklidir.")
                .MaximumLength(MaxFileNameLength)
                .WithMessage($"Dosya adı en fazla {MaxFileNameLength} karakter olabilir.");

            RuleFor(x => x.Receipt!)
                .Must(HaveAllowedExtension)
                .WithMessage("Yalnızca PDF, JPG, JPEG ve PNG dosyaları kabul edilir.");
        });
    }

    private static bool HaveAllowedExtension(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        return AllowedExtensions.Contains(extension)
            && AllowedContentTypes.Contains(file.ContentType);
    }
}