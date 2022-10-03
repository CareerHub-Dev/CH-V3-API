using Application.Common.DTO.Experiences;
using Application.Experiences.Commands.CreateExperience;
using Application.Experiences.Commands.DeleteExperience;
using Application.Experiences.Commands.UpdateExperience;
using Application.Experiences.Queries.GetExperience;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class ExperiencesController : ApiControllerBase
{
    [HttpGet("{experienceId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExperienceDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExperience(Guid experienceId)
    {
        return Ok(await Mediator.Send(new GetExperienceWithFilterQuery { ExperienceId = experienceId }));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateExperience(CreateExperienceCommand command)
    {
        var result = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetExperience), new { experienceId = result }, result);
    }

    [HttpDelete("{experienceId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteExperience(Guid experienceId)
    {
        await Mediator.Send(new DeleteExperienceCommand(experienceId));

        return NoContent();
    }

    [HttpPut("{experienceId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateExperience(Guid experienceId, UpdateExperienceCommand command)
    {
        if (experienceId != command.ExperienceId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
}
