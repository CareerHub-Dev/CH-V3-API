using Application.Students.Commands.DeleteStudent;
using Application.Students.Commands.UpdateStudent;
using Application.Students.Commands.UpdateStudentPhoto;
using Application.Students.Queries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.Common.Extentions;
using WebUI.Common.Models;
using WebUI.Common.Models.Student;

namespace WebUI.Areas.Company;

[Authorize("Company")]
[Route("api/Company/[controller]")]
public class StudentsController : ApiControllerBase
{
    /// <remarks>
    /// Company
    /// 
    ///     get all Verified Brief students
    ///     
    /// </remarks>
    [HttpGet]
    public async Task<IEnumerable<StudentBriefResponse>> GetStudents(
        [FromQuery] PaginationParameters paginationParameters,
        [FromQuery] SearchParameter searchParameter)
    {
        var result = await Mediator.Send(new GetStudentBriefsWithPaginationWithSearthWithFilterQuery
        {
            PageNumber = paginationParameters.PageNumber,
            PageSize = paginationParameters.PageSize,
            SearchTerm = searchParameter.SearchTerm,
            IsVerified = true
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return result.Select(x => new StudentBriefResponse(x));
    }

    /// <remarks>
    /// Company
    /// 
    ///     get Verified Detailed student
    ///     
    /// </remarks>
    [HttpGet("{studentId}")]
    public async Task<StudentDetailedResponse> GetStudent(Guid studentId)
    {
        var result = await Mediator.Send(new GetStudentDetailedWithFilterQuery
        {
            StudentId = studentId,
            IsVerified = true
        });

        return new StudentDetailedResponse(result);
    }
}
