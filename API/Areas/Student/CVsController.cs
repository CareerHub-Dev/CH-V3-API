using API.Authorize;
using API.DTO.Requests.CVs;
using API.DTO.Responses;
using Application.Common.DTO.CVs;
using Application.Common.DTO.Experiences;
using Application.Common.DTO.JobOfferReviews;
using Application.Common.Enums;
using Application.CVs.Commands.CreateCV;
using Application.CVs.Commands.DeleteCVOfStudent;
using Application.CVs.Commands.SendCVOfStudentForJobOffer;
using Application.CVs.Commands.UpdateCVDetailOfStudent;
using Application.CVs.Commands.UpdateCVPhotoOfStudent;
using Application.CVs.Queries.GetBriefCVsOfStudentWithPaging;
using Application.CVs.Queries.GetCVOfStudent;
using Application.CVs.Queries.GetCVWord;
using Application.CVs.Queries.GetCVWordOfStudent;
using Application.Experiences.Queries.GetExperienceOfStudent;
using Application.JobOfferReviews.Queries.GetJobOfferReviewOfStudent;
using Application.JobOfferReviews.Queries.GetJobOfferReviewsOfStudentWithPaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas.Student;

[Authorize(Role.Student)]
[Route("api/Student/self/[controller]")]
public class CVsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BriefCVDTO>))]
    public async Task<IActionResult> GetCVsOfSelfStudent(
        [FromQuery] string? order,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetBriefCVsOfStudentWithPagingQuery
        {
            StudentId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,

            OrderByExpression = order ?? "Title"
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("reviews")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<JobOfferReviewDTO>))]
    public async Task<IActionResult> GetJobOfferReviewsOfSelfStudent(
        [FromQuery] string? order,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetJobOfferReviewsOfStudentWithPagingQuery
        {
            StudentId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,

            OrderByExpression = order ?? "Created DESC"
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("reviews/{reviewId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobOfferReviewDTO))]
    public async Task<IActionResult> GetJobOfferReviewOfSelfStudent(
        Guid reviewId)
    {
        var result = await Sender.Send(new GetJobOfferReviewOfStudentQuery
        {
            StudentId = AccountInfo!.Id,
            ReviewId = reviewId
        });

        return Ok(result);
    }

    [HttpGet("{cvId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CVDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCVOfSelfStudent(Guid cvId)
    {
        return Ok(await Sender.Send(new GetCVOfStudentQuery
        {
            CVId = cvId,
            StudentId = AccountInfo!.Id
        }));
    }

    private static readonly string WordProcessingMlFormat = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

    [HttpGet("{id}/word")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileResult))]
    public async Task<FileResult> GetCVWordOfSelfStudent(Guid id)
    {
        return File(await Sender.Send(new GetCVWordOfStudentQuery
        {
            CVId = id,
            StudentId = AccountInfo!.Id
        }), WordProcessingMlFormat, "my-cv.docx");
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> CreateCVForSelfStudent([FromForm] CreateOwnCVRequest request)
    {
        var result = await Sender.Send(new CreateCVCommand
        {
            ExperienceLevel = request.ExperienceLevel,
            Title = request.Title,
            JobPositionId = request.JobPositionId,
            TemplateLanguage = request.TemplateLanguage,
            LastName = request.LastName,
            FirstName = request.FirstName,
            Photo = request.Photo,
            Goals = request.Goals,
            HardSkills = request.HardSkills,
            SoftSkills = request.SoftSkills,
            ForeignLanguages = request.ForeignLanguages,
            ProjectLinks = request.ProjectLinks,
            Educations = request.Educations,
            Experiences = request.Experiences,
            StudentId = AccountInfo!.Id
        });

        return Ok(result);
    }

    [HttpPost("without-photo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> CreateCVWithoutPhotoForSelfStudent([FromBody] CreateOwnCVRequest request)
    {
        var result = await Sender.Send(new CreateCVWithoutPhotoCommand
        {
            ExperienceLevel = request.ExperienceLevel,
            Title = request.Title,
            JobPositionId = request.JobPositionId,
            TemplateLanguage = request.TemplateLanguage,
            LastName = request.LastName,
            FirstName = request.FirstName,
            Goals = request.Goals,
            HardSkills = request.HardSkills,
            SoftSkills = request.SoftSkills,
            ForeignLanguages = request.ForeignLanguages,
            ProjectLinks = request.ProjectLinks,
            Educations = request.Educations,
            Experiences = request.Experiences,
            StudentId = AccountInfo!.Id
        });

        return Ok(result);
    }

    [HttpPut("{cvId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCVDetailOfSelfStudent(Guid cvId, UpdateOwnCVDetailRequest request)
    {
        await Sender.Send(new UpdateCVDetailOfStudentCommand
        {
            CVId = cvId,
            Title = request.Title,
            ExperienceLevel = request.ExperienceLevel,
            JobPositionId = request.JobPositionId,
            TemplateLanguage = request.TemplateLanguage,
            LastName = request.LastName,
            FirstName = request.FirstName,
            Goals = request.Goals,
            HardSkills = request.HardSkills,
            SoftSkills = request.SoftSkills,
            ForeignLanguages = request.ForeignLanguages,
            ProjectLinks = request.ProjectLinks,
            Educations = request.Educations,
            Experiences = request.Experiences,
            StudentId = AccountInfo!.Id
        });

        return NoContent();
    }

    [HttpPost("{cvId}/photo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageResponse))]
    public async Task<IActionResult> UpdateCVPhotoOfSelfStudent(Guid cvId, IFormFile? file)
    {
        var result = await Sender.Send(new UpdateCVPhotoOfStudentCommand
        {
            CVId = cvId,
            StudentId = AccountInfo!.Id,
            Photo = file
        });

        return Ok(new ImageResponse { Route = result });
    }

    [HttpPost("send-for-joboffer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendCVOfSelfStudentForJobOffer(SendCVForJobOfferRequest request)
    {
        await Sender.Send(new SendCVOfStudentForJobOfferCommand
        {
            CVId = request.CVId,
            StudentId = AccountInfo!.Id,
            JobOfferId = request.JobOfferId,
            IsJobOfferMustBeActive = true,
            IsCompanyOfJobOfferMustBeVerified = true,
        });

        return NoContent();
    }

    [HttpDelete("{cvId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCVOfSelfStudent(Guid cvId)
    {
        await Sender.Send(new DeleteCVOfStudentCommand(cvId, AccountInfo!.Id));

        return NoContent();
    }
}
