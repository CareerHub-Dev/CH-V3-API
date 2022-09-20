using Application.Enums.Queries.GetDegrees;
using Application.Enums.Queries.GetExperienceLevels;
using Application.Enums.Queries.GetJobTypes;
using Application.Enums.Queries.GetLanguageLevels;
using Application.Enums.Queries.GetTemplateLanguages;
using Application.Enums.Queries.GetWorkFormats;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Auth;

[Authorize]
[Route("api/Auth")]
public class EnumsController : ApiControllerBase
{
    [HttpGet("Degrees")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<DegreeDTO>))]
    public async Task<IActionResult> GetDegrees()
    {
        return Ok(await Mediator.Send(new GetDegreesQuery()));
    }

    [HttpGet("ExperienceLevels")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ExperienceLevelDTO>))]
    public async Task<IActionResult> GetExperienceLevels()
    {
        return Ok(await Mediator.Send(new GetExperienceLevelsQuery()));
    }

    [HttpGet("JobTypes")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<JobTypeDTO>))]
    public async Task<IActionResult> GetJobTypes()
    {
        return Ok(await Mediator.Send(new GetJobTypesQuery()));
    }

    [HttpGet("LanguageLevels")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<JobTypeDTO>))]
    public async Task<IActionResult> GetLanguageLevels()
    {
        return Ok(await Mediator.Send(new GetLanguageLevelsQuery()));
    }

    [HttpGet("TemplateLanguages")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<TemplateLanguageDTO>))]
    public async Task<IActionResult> GetTemplateLanguages()
    {
        return Ok(await Mediator.Send(new GetTemplateLanguagesQuery()));
    }

    [HttpGet("WorkFormats")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<WorkFormatDTO>))]
    public async Task<IActionResult> GetWorkFormats()
    {
        return Ok(await Mediator.Send(new GetWorkFormatsQuery()));
    }
}
