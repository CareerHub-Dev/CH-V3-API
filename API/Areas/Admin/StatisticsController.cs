using API.Authorize;
using Application.Common.DTO.Tags;
using Application.Common.Enums;
using Application.Statistics.Query.GetReviewsAnalitics;
using Application.Statistics.Query.GetTagsAnalitics;
using Application.Tags.Queries.GetTagsWithPaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas.Admin;

[Authorize(Role.Admin, Role.SuperAdmin)]
[Route("api/Admin/[controller]")]
public class StatisticsController : ApiControllerBase
{
    [HttpGet("ReviewsAnalitics")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewsAnaluticsResult))]
    public async Task<IActionResult> GetReviewsAnalitics([FromQuery] ReviewsAnaliticRange range)
    {
        var result = await Sender.Send(new GetReviewsAnaliticsQuery
        {
            Range = range
        });

        return Ok(result);
    }

    [HttpGet("TagsAnalitics")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TagsAnaliticsResult))]
    public async Task<IActionResult> GetTagsAnalitics([FromQuery] List<Guid>? ids)
    {
        var result = await Sender.Send(new GetTagsAnaliticsQuery
        {
            Ids = ids
        });

        return Ok(result);
    }
}
