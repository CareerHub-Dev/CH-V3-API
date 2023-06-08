using API.Authorize;
using Application.Common.DTO.CVs;
using Application.Common.DTO.JobOfferReviews;
using Application.Common.Enums;
using Application.CVs.Queries.GetCVOfStudent;
using Application.CVs.Queries.GetCVWord;
using Application.JobOfferReviews.Queries.GetJobOfferReviewOfStudent;
using Application.JobOfferReviews.Queries.GetReviewDetailsAsCompany;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Company;

[Authorize(Role.Company)]
[Route("api/Company/[controller]")]
public class CVsController : ApiControllerBase
{
    private static readonly string WordProcessingMlFormat = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

    [HttpGet("{id}/word")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileResult))]
    public async Task<FileResult> GetCVWord(Guid id)
    {
        return File(await Sender.Send(new GetCVWordQuery(id)), WordProcessingMlFormat, "cv.docx");
    }

    [HttpGet("{cvId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CVDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAppliedCVById(Guid cvId)
    {
        return Ok(await Sender.Send(new GetAppliedCVQuery
        {
            CVId = cvId,
        }));
    }

    [HttpGet("reviews/{reviewId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DetailedJobOfferReviewDTO))]
    public async Task<IActionResult> GetJobOfferReviewAsCompany(Guid reviewId)
    {
        var result = await Sender.Send(new GetReviewDetailsAsCompanyQuery
        {
            CompanyId = AccountInfo!.Id,
            ReviewId = reviewId
        });

        return Ok(result);
    }
}
