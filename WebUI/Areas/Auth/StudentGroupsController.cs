using Application.StudentGroups.Queries;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Models;
using WebUI.Common.Models.StudentGroup;

namespace WebUI.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class StudentGroupsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<StudentGroupBriefResponse>> GetStudentGroups([FromQuery] SearchParameter searchParameter)
    {
        var result = await Mediator.Send(new GetStudentGroupBriefsWithSearchQuery
        {
            SearchTerm = searchParameter.SearchTerm
        });

        return result.Select(x => new StudentGroupBriefResponse(x));
    }

    [HttpGet("{studentGroupId}")]
    public async Task<StudentGroupBriefResponse> GetStudentGroup(Guid studentGroupId)
    {
        var result = await Mediator.Send(new GetStudentGroupBriefQuery(studentGroupId));

        return new StudentGroupBriefResponse(result);
    }
}
