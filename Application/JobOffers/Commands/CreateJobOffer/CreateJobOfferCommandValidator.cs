using Application.Common.Entensions;
using FluentValidation;

namespace Application.JobOffers.Commands.CreateJobOffer;

public class CreateJobOfferCommandValidator
    : AbstractValidator<CreateJobOfferCommand>
{
    public CreateJobOfferCommandValidator()
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
            .GreaterThan(x => x.StartDate.Date).OverridePropertyName("EndDate")
            .LessThanOrEqualTo(x => x.StartDate.AddDays(60).Date).OverridePropertyName("EndDate");
    }
}
