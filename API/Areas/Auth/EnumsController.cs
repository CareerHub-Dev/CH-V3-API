using API.Authorize;
using Application.Common.DTO;
using Application.Enums.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Auth;

[Authorize]
[Route("api/Auth")]
public class EnumsController : ApiControllerBase
{
    [HttpGet("Degrees")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EnumDTO>))]
    public async Task<IActionResult> GetDegrees()
    {
        return Ok(await Sender.Send(new GetDegreesQuery()));
    }

    [HttpGet("ExperienceLevels")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EnumDTO>))]
    public async Task<IActionResult> GetExperienceLevels()
    {
        return Ok(await Sender.Send(new GetExperienceLevelsQuery()));
    }

    [HttpGet("JobTypes")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EnumDTO>))]
    public async Task<IActionResult> GetJobTypes()
    {
        return Ok(await Sender.Send(new GetJobTypesQuery()));
    }

    [HttpGet("LanguageLevels")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EnumDTO>))]
    public async Task<IActionResult> GetLanguageLevels()
    {
        return Ok(await Sender.Send(new GetLanguageLevelsQuery()));
    }

    [HttpGet("TemplateLanguages")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EnumDTO>))]
    public async Task<IActionResult> GetTemplateLanguages()
    {
        return Ok(await Sender.Send(new GetTemplateLanguagesQuery()));
    }

    [HttpGet("WorkFormats")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EnumDTO>))]
    public async Task<IActionResult> GetWorkFormats()
    {
        return Ok(await Sender.Send(new GetWorkFormatsQuery()));
    }
}
