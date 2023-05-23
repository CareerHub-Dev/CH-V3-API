using API.Authorize;
using Application.Common.Enums;
using Application.CVs.Queries.GetCVWord;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Company;

[Authorize(Role.Company)]
[Route("api/Company/[controller]")]
public class CVsController : ApiControllerBase
{
    private static readonly string WordProcessingMlFormat = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

    [HttpGet("{id}/word")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileResult))]
    public async Task<FileResult> GetCVWord(Guid id)
    {
        return File(await Sender.Send(new GetCVWordQuery(id)), WordProcessingMlFormat, "cv.docx");
    }
}
