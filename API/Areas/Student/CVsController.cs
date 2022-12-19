using API.Authorize;
using API.DTO.Requests.CVs;
using API.DTO.Responses;
using Application.Common.Enums;
using Application.CVs.Commands.CreateCV;
using Application.CVs.Commands.DeleteCVOfStudent;
using Application.CVs.Commands.UpdateCVDetailOfStudent;
using Application.CVs.Commands.UpdateCVPhotoOfStudent;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Student;

[Authorize(Role.Student)]
[Route("api/Student/self/[controller]")]
public class CVsController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> CreateCVForSelfStudent(CreateOwnCVRequest request)
    {
        var result = await Sender.Send(new CreateCVCommand
        {
            Title = request.Title,
            JobPositionId = request.JobPositionId,
            TemplateLanguage = request.TemplateLanguage,
            LastName = request.LastName,
            FirstName = request.FirstName,
            Photo = request.Photo,
            Goals = request.Goals,
            SkillsAndTechnologies = request.SkillsAndTechnologies,
            ExperienceHighlights = request.ExperienceHighlights,
            ForeignLanguages = request.ForeignLanguages,
            ProjectLinks = request.ProjectLinks,
            Educations = request.Educations,
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
        if (cvId != request.CVId)
        {
            return BadRequest();
        }

        await Sender.Send(new UpdateCVDetailOfStudentCommand
        {
            CVId = request.CVId,
            JobPositionId = request.JobPositionId,
            TemplateLanguage = request.TemplateLanguage,
            LastName = request.LastName,
            FirstName = request.FirstName,
            Goals = request.Goals,
            SkillsAndTechnologies = request.SkillsAndTechnologies,
            ExperienceHighlights = request.ExperienceHighlights,
            ForeignLanguages = request.ForeignLanguages,
            ProjectLinks = request.ProjectLinks,
            Educations = request.Educations,
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

    [HttpDelete("{cvId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCVOfSelfStudent(Guid cvId)
    {
        await Sender.Send(new DeleteCVOfStudentCommand(cvId, AccountInfo!.Id));

        return NoContent();
    }
}
