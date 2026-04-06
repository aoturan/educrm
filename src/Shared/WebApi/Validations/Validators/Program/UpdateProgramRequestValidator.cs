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

        RuleFor(x => x.PublicScheduleText)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.PublicDetailedDescription)
            .MaximumLength(2000)
            .When(x => x.PublicDetailedDescription is not null);

        RuleFor(x => x.LocationDetails)
            .MaximumLength(500)
            .When(x => x.LocationDetails is not null);

        RuleFor(x => x.OnlineParticipationInfo)
            .MaximumLength(1000)
            .When(x => x.OnlineParticipationInfo is not null);

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
            .GreaterThan(0)
            .WithMessage("PriceAmount must be greater than 0.")
            .When(x => x.PriceAmount is not null);

        RuleFor(x => x.PriceCurrency)
            .NotNull()
            .WithMessage("PriceCurrency is required when PriceAmount is provided.")
            .IsInEnum()
            .WithMessage("PriceCurrency must be a valid currency (TRY, USD, EUR).")
            .When(x => x.PriceAmount is not null);

        RuleFor(x => x.PriceNote)
            .MaximumLength(1000)
            .When(x => x.PriceNote is not null);
    }
}

