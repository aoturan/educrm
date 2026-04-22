using EduCrm.WebApi.Contracts.Program;
using FluentValidation;

namespace EduCrm.WebApi.Validations.Validators.Program;

public sealed class UpdateProgramRequestValidator : AbstractValidator<UpdateProgramRequest>
{
    public UpdateProgramRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.StartDate)
            .NotEmpty();

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("EndDate must be on or after StartDate.");

        RuleFor(x => x.PublicShortDescription)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.PublicModality)
            .IsInEnum();

        RuleFor(x => x.PriceType)
            .IsInEnum()
            .WithMessage("PriceType is required and must be a valid value (Free, Paid, ContactForPrice).");

        RuleFor(x => x.PublicScheduleText)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.PublicDetailedDescription)
            .MaximumLength(2000)
            .When(x => x.PublicDetailedDescription is not null);

        RuleFor(x => x.OnlineParticipationInfo)
            .NotEmpty()
            .WithMessage("Online modda katılım bilgisi girilmelidir.")
            .MaximumLength(1000)
            .When(x => x.PublicModality == EduCrm.Modules.Program.Domain.Enums.ProgramModality.Online);

        RuleFor(x => x.LocationDetails)
            .NotEmpty()
            .WithMessage("Yüz yüze/Hibrit modda konum bilgisi girilmelidir.")
            .MaximumLength(500)
            .When(x => x.PublicModality != EduCrm.Modules.Program.Domain.Enums.ProgramModality.Online);

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("Capacity must be greater than 0.")
            .When(x => x.Capacity is not null);

        RuleFor(x => x.PublicInstructorName)
            .MaximumLength(200)
            .When(x => x.PublicInstructorName is not null);

        RuleFor(x => x.PublicEnrollmentDeadline)
            .Cascade(CascadeMode.Stop)
            .LessThanOrEqualTo(x => x.StartDate)
            .WithMessage("PublicEnrollmentDeadline cannot be after StartDate.")
            .LessThanOrEqualTo(x => x.EndDate)
            .WithMessage("PublicEnrollmentDeadline cannot be after EndDate.")
            .When(x => x.PublicEnrollmentDeadline is not null);

        RuleFor(x => x.PriceAmount)
            .NotNull()
            .WithMessage("PriceAmount is required when PriceType is Paid.")
            .GreaterThan(0)
            .WithMessage("PriceAmount must be greater than 0.")
            .LessThanOrEqualTo(999999)
            .WithMessage("PriceAmount cannot exceed 999999.")
            .When(x => x.PriceType == EduCrm.Modules.Program.Domain.Enums.ProgramPriceType.Paid);

        RuleFor(x => x.PriceCurrency)
            .NotNull()
            .WithMessage("PriceCurrency is required when PriceType is Paid.")
            .IsInEnum()
            .WithMessage("PriceCurrency must be a valid currency (TRY, USD, EUR).")
            .When(x => x.PriceType == EduCrm.Modules.Program.Domain.Enums.ProgramPriceType.Paid);

        RuleFor(x => x.PriceNote)
            .MaximumLength(1000)
            .When(x => x.PriceNote is not null);
    }
}

