using API.Authorize;
using Application.CVs.Queries.GetHardSkills;
using Application.CVs.Queries.GetSoftSkills;
using Application.Enums.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class CVsController : ApiControllerBase
{
    [HttpGet("HardSkills")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHardSkills()
    {
        return Ok(new
        {
            HardSkills = await Sender.Send(new GetHardSkillsQuery())
        });
    }

    [HttpGet("SoftSkills")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSoftSkills()
    {
        return Ok(new
        {
            SoftSkills = await Sender.Send(new GetSoftSkillsQuery())
        });
    }
}
