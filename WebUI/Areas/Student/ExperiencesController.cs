using Application.Experiences.Commands.CreateExperience;
using Application.Experiences.Commands.DeleteExperienceOfStudent;
using Application.Experiences.Commands.UpdateExperienceOfStudent;
using Application.Experiences.Queries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Models.Experience;

namespace WebUI.Areas.Student;

[Authorize("Student")]
[Route("api/Student/[controller]")]
public class ExperiencesController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ExperienceDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExperiencesOfStudent(
        Guid studentId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetExperiencesOfStudentWithPaginationWithFilterQuery
        {
            StudentId = AccountInfo!.Id,
            IsStudentMustBeVerified = true,
            PageNumber = pageNumber,
            PageSize = pageSize,
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{experienceId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExperienceDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExperience(Guid experienceId)
    {
        return Ok(await Mediator.Send(new GetExperienceOfStudentQuery
        {
            ExperienceId = experienceId,
            StudentId = AccountInfo!.Id,
            IsStudentMustBeVerified = true
        }));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateExperience(CreateExperienceView view)
    {
        var result = await Mediator.Send(new CreateExperienceCommand
        {
            Title = view.Title,
            CompanyName = view.CompanyName,
            JobType = view.JobType,
            WorkFormat = view.WorkFormat,
            ExperienceLevel = view.ExperienceLevel,
            JobLocation = view.JobLocation,
            StartDate = view.StartDate,
            EndDate = view.EndDate,
            StudentId = AccountInfo!.Id
        });

        return CreatedAtAction(nameof(GetExperience), new { experienceId = result }, result);
    }

    [HttpDelete("{experienceId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteExperience(Guid experienceId)
    {
        await Mediator.Send(new DeleteExperienceOfStudentCommand(experienceId, AccountInfo!.Id));

        return NoContent();
    }

    [HttpPut("{experienceId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateExperience(Guid experienceId, UpdateExperienceView view)
    {
        if (experienceId != view.ExperienceId)
        {
            return BadRequest();
        }

        await Mediator.Send(new UpdateExperienceOfStudentCommand
        {
            ExperienceId = view.ExperienceId,
            Title = view.Title,
            CompanyName = view.CompanyName,
            JobType = view.JobType,
            WorkFormat = view.WorkFormat,
            ExperienceLevel = view.ExperienceLevel,
            JobLocation = view.JobLocation,
            StartDate = view.StartDate,
            EndDate = view.EndDate,
            StudentId = AccountInfo!.Id
        });

        return NoContent();
    }
}
