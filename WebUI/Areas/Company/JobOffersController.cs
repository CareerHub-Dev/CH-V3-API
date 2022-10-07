using Application.JobOffers.Commands.CreateJobOffer;
using Application.JobOffers.Commands.DeleteJobOfferOfCompany;
using Application.JobOffers.Commands.UpdateJobOfferDetailOfCompany;
using Application.JobOffers.Commands.UpdateJobOfferImageOfCompany;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.DTO.Requests.JobOffers;

namespace WebUI.Areas.Company;

[Authorize("Company")]
[Route("api/Company/self/[controller]")]
public class JobOffersController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateJobOfferForSelfCompany(CreateJobOfferRequest view)
    {
        var result = await Mediator.Send(new CreateJobOfferCommand
        {
            Title = view.Title,
            Overview = view.Overview,
            Requirements = view.Requirements,
            Responsibilities = view.Responsibilities,
            Preferences = view.Preferences,
            Image = view.Image,
            JobType = view.JobType,
            WorkFormat = view.WorkFormat,
            ExperienceLevel = view.ExperienceLevel,
            StartDate = view.StartDate,
            EndDate = view.EndDate,
            JobPositionId = view.JobPositionId,
            TagIds = view.TagIds,

            CompanyId = AccountInfo!.Id
        });

        return Ok(result);
    }

    [HttpDelete("{jobOfferId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteJobOfferOfSelfCompany(Guid jobOfferId)
    {
        await Mediator.Send(new DeleteJobOfferOfCompanyCommand(jobOfferId, AccountInfo!.Id));

        return NoContent();
    }

    [HttpPut("{jobOfferId}/detail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateJobOfferDetailOfSelfCompany(Guid jobOfferId, UpdateJobOfferDetailRequest view)
    {
        if (jobOfferId != view.JobOfferId)
        {
            return BadRequest();
        }

        await Mediator.Send(new UpdateJobOfferDetailOfCompanyCommand
        {
            JobOfferId = view.JobOfferId,
            Title = view.Title,
            Overview = view.Overview,
            Requirements = view.Requirements,
            Responsibilities = view.Responsibilities,
            Preferences = view.Preferences,
            JobType = view.JobType,
            WorkFormat = view.WorkFormat,
            ExperienceLevel = view.ExperienceLevel,
            StartDate = view.StartDate,
            EndDate = view.EndDate,
            JobPositionId = view.JobPositionId,
            TagIds = view.TagIds,

            CompanyId = AccountInfo!.Id
        });

        return NoContent();
    }

    [HttpPost("{jobOfferId}/image")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid?))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateJobOfferImageOfSelfCompany(Guid jobOfferId, IFormFile? file)
    {
        var result = await Mediator.Send(new UpdateJobOfferImageOfCompanyCommand 
        { 
            CompanyId = AccountInfo!.Id, 

            JobofferId = jobOfferId,
            Image = file 
        });

        return Ok(result);
    }
}
