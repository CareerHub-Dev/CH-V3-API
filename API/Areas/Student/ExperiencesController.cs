using API.Authorize;
using API.DTO.Requests.Experiences;
using Application.Common.DTO.Experiences;
using Application.Common.Enums;
using Application.Experiences.Commands.CreateExperience;
using Application.Experiences.Commands.DeleteExperienceOfStudent;
using Application.Experiences.Commands.UpdateExperienceOfStudent;
using Application.Experiences.Queries.GetExperience;
using Application.Experiences.Queries.GetExperiences;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas.Student;

[Authorize(Role.Student)]
[Route("api/Student/self/[controller]")]
public class ExperiencesController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ExperienceDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExperiencesOfSelfStudent(
        [FromQuery] string? orderByExpression,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetExperiencesOfStudentWithPaginationWithFilterWithSortQuery
        {
            StudentId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,

            OrderByExpression = orderByExpression ?? "Title"
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{experienceId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExperienceDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExperienceOfSelfStudent(Guid experienceId)
    {
        return Ok(await Mediator.Send(new GetExperienceOfStudentWithFilterQuery
        {
            ExperienceId = experienceId,
            StudentId = AccountInfo!.Id
        }));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateExperienceForSelfStudent(CreateExperienceRequest view)
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

        return Ok(result);
    }

    [HttpDelete("{experienceId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteExperienceOfSelfStudent(Guid experienceId)
    {
        await Mediator.Send(new DeleteExperienceOfStudentCommand(experienceId, AccountInfo!.Id));

        return NoContent();
    }

    [HttpPut("{experienceId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateExperienceOfSelfStudent(Guid experienceId, UpdateExperienceRequest view)
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
