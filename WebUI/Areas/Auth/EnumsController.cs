using Application.Common.DTO;
using Application.Enums.Queries;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Auth;

[Authorize]
[Route("api/Auth")]
public class EnumsController : ApiControllerBase
{
    [HttpGet("Degrees")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<EnumDTO>))]
    public async Task<IActionResult> GetDegrees()
    {
        return Ok(await Mediator.Send(new GetDegreesQuery()));
    }

    [HttpGet("ExperienceLevels")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<EnumDTO>))]
    public async Task<IActionResult> GetExperienceLevels()
    {
        return Ok(await Mediator.Send(new GetExperienceLevelsQuery()));
    }

    [HttpGet("JobTypes")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<EnumDTO>))]
    public async Task<IActionResult> GetJobTypes()
    {
        return Ok(await Mediator.Send(new GetJobTypesQuery()));
    }

    [HttpGet("LanguageLevels")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<EnumDTO>))]
    public async Task<IActionResult> GetLanguageLevels()
    {
        return Ok(await Mediator.Send(new GetLanguageLevelsQuery()));
    }

    [HttpGet("TemplateLanguages")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<EnumDTO>))]
    public async Task<IActionResult> GetTemplateLanguages()
    {
        return Ok(await Mediator.Send(new GetTemplateLanguagesQuery()));
    }

    [HttpGet("WorkFormats")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<EnumDTO>))]
    public async Task<IActionResult> GetWorkFormats()
    {
        return Ok(await Mediator.Send(new GetWorkFormatsQuery()));
    }

    [HttpGet("ActivationStatuses")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<EnumDTO>))]
    public async Task<IActionResult> GetActivationStatuses()
    {
        return Ok(await Mediator.Send(new GetActivationStatusesQuery()));
    }
}
