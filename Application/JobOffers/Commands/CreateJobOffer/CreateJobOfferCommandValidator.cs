using Application.Common.Entensions;
using FluentValidation;

namespace Application.JobOffers.Commands.CreateJobOffer;

public class CreateJobOfferCommandValidator : AbstractValidator<CreateJobOfferCommand>
{
    public CreateJobOfferCommandValidator()
    {
        When(x => x.Image != null, () =>
        {
            RuleFor(x => x.Image!)
                .AllowedExtensions(".jpg", ".jpeg", ".png")
                .MaxFileSize(2 * 1024 * 1024);
        });

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
            .GreaterThan(x => x.StartDate.Date.AddDays(60)).OverridePropertyName("EndDate");
    }
}
