using Application.Common.Models.Pagination;
using Application.StudentGroups.Commands.CreateStudentGroup;
using Application.StudentGroups.Commands.DeleteStudentGroup;
using Application.StudentGroups.Commands.UpdateStudentGroup;
using Application.StudentGroups.Queries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Models;
using WebUI.Common.Models.StudentGroup;

namespace WebUI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentGroupsController : ApiControllerBase
{
    /// <summary>
    /// Auth
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<StudentGroupBriefResponse>> GetStudentGroups(
        [FromQuery] PaginationParameters paginationParameters,
        [FromQuery] SearchParameter searchParameter)
    {
        switch (AccountInfo!.Role)
        {
            case "Admin":
                {
                    var result = await Mediator.Send(new GetStudentGroupsWithPaginationWithSearchQuery
                    {
                        PaginationParameters = paginationParameters,
                        SearchTerm = searchParameter.SearchTerm,
                    });

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

                    return result.Select(x => new StudentGroupResponse(x));
                }
            default:
                {
                    var result = await Mediator.Send(new GetStudentGroupBriefsWithSearchQuery
                    {
                        SearchTerm = searchParameter.SearchTerm
                    });

                    return result.Select(x => new StudentGroupBriefResponse(x));
                }
        }
    }

    /// <summary>
    /// Auth
    /// </summary>
    [HttpGet("{studentGroupId}")]
    [Authorize]
    public async Task<StudentGroupBriefResponse> GetStudentGroup(Guid studentGroupId)
    {
        switch (AccountInfo!.Role)
        {
            case "Admin":
                {
                    var result = await Mediator.Send(new GetStudentGroupQuery(studentGroupId));

                    return new StudentGroupResponse(result);
                }
            default:
                {
                    var result = await Mediator.Send(new GetStudentGroupBriefQuery(studentGroupId));

                    return new StudentGroupBriefResponse(result);
                }
        }
    }

    /// <summary>
    /// Admin
    /// </summary>
    [HttpDelete("{studentGroupId}")]
    [Authorize("Admin")]
    public async Task<IActionResult> DeleteStudentGroup(Guid studentGroupId)
    {
        await Mediator.Send(new DeleteStudentGroupCommand(studentGroupId));
        return NoContent();
    }

    /// <summary>
    /// Admin
    /// </summary>
    [HttpPost]
    [Authorize("Admin")]
    public async Task<Guid> CreateStudentGroup(CreateStudentGroupRequest createStudentGroup)
    {
        return await Mediator.Send(new CreateStudentGroupCommand { Name = createStudentGroup.Name });
    }

    /// <summary>
    /// Admin
    /// </summary>
    [HttpPut("{studentGroupId}")]
    [Authorize("Admin")]
    public async Task<IActionResult> UpdateStudentGroup(Guid studentGroupId, UpdateStudentGroupRequest updateStudentGroup)
    {
        await Mediator.Send(new UpdateStudentGroupCommand { StudentGroupId = studentGroupId, Name = updateStudentGroup.Name });
        return NoContent();
    }
}
