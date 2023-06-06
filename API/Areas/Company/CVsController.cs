using API.Authorize;
using Application.Common.DTO.CVs;
using Application.Common.DTO.JobOfferReviews;
using Application.Common.Enums;
using Application.CVs.Queries.GetCVOfStudent;
using Application.CVs.Queries.GetCVWord;
using Application.JobOfferReviews.Queries.GetJobOfferReviewsOfStudentWithPaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

    [HttpGet("{jobOfferId}/{cvId}/reviews")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<JobOfferReviewDTO>))]
    public async Task<IActionResult> GetAppliedCVReviews(Guid jobOfferId, Guid cvId)
    {
        var result = await Sender.Send(new GetReviewsOfAppliedCVQuery
        {
            JobOfferId = jobOfferId,
            CvId = cvId,
        });

        return Ok(result);
    }
}
