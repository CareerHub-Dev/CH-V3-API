using FluentValidation;

namespace Application.Experiences.Commands.CreateExperience;

public class CreateExperienceCommandValidator : AbstractValidator<CreateExperienceCommand>
{
    public CreateExperienceCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.JobLocation)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.JobType)
            .IsInEnum();

        RuleFor(x => x.WorkFormat)
            .IsInEnum();

        RuleFor(x => x.ExperienceLevel)
            .IsInEnum();

        RuleFor(x => x.StartDate).LessThan(x => x.EndDate).When(x => x.EndDate.HasValue);
    }
}
