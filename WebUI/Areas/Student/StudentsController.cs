using Application.Students.Queries.GetStudents;
using Application.Students.Queries.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Models.Student;

namespace WebUI.Areas.Student;

[Authorize("Student")]
[Route("api/Student/[controller]")]
public class StudentsController : ApiControllerBase
{

    [HttpGet]
    public async Task<IEnumerable<FollowedStudentDetailedDTO>> GetStudents(
        [FromQuery] GetStudentsWithPaginationWithSearthWithFilterForStudentView view)
    {
        var result = await Mediator.Send(new GetFollowedStudentDetailedsForFollowerStudentWithPaginationWithSearchWithFilterQuery
        {
            FollowerStudentId = AccountInfo!.Id,
            IsFollowerStudentVerified = true,
            PageNumber = view.PageNumber,
            PageSize = view.PageSize,
            SearchTerm = view.SearchTerm,
            IsVerified = true,
            WithoutStudentId = AccountInfo!.Id,
            StudentGroupIds = view.StudentGroupIds,
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return result;
    }
}
