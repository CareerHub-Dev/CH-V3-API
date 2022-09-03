using Application.Common.Models.StudentGroup;
using Application.StudentGroups.Queries;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;

namespace WebUI.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class StudentGroupsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<StudentGroupBriefDTO>> GetStudentGroups([FromQuery] GetStudentGroupBriefsWithSearchQuery query)
    {
        return await Mediator.Send(query);
    }
}
