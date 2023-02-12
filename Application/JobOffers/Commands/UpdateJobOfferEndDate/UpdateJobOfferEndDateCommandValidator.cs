using FluentValidation;

namespace Application.JobOffers.Commands.UpdateJobOfferEndDate;

public class UpdateJobOfferEndDateCommandValidator
    : AbstractValidator<UpdateJobOfferEndDateCommand>
{
    public UpdateJobOfferEndDateCommandValidator()
    {
        RuleFor(x => x.EndDate.Date)
            .GreaterThan(DateTime.Today.AddDays(60)).OverridePropertyName("EndDate");
    }
}
