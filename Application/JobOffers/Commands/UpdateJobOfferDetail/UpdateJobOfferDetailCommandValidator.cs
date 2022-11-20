using FluentValidation;

namespace Application.JobOffers.Commands.UpdateJobOfferDetail;

public class UpdateJobOfferDetailCommandValidator
    : AbstractValidator<UpdateJobOfferDetailCommand>
{
    public UpdateJobOfferDetailCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.Overview)
            .NotEmpty()
            .MaximumLength(1024);

        RuleFor(x => x.Requirements)
            .NotEmpty()
            .MaximumLength(1024);

        RuleFor(x => x.Responsibilities)
            .NotEmpty()
            .MaximumLength(1024);

        RuleFor(x => x.JobType)
            .IsInEnum();

        RuleFor(x => x.WorkFormat)
            .IsInEnum();

        RuleFor(x => x.ExperienceLevel)
            .IsInEnum();

        RuleFor(x => x.EndDate.Date)
            .GreaterThan(DateTime.Today.AddDays(60)).OverridePropertyName("EndDate");
    }
}
