using API.Authorize;
using Application.Common.DTO.Posts;
using Application.Common.Enums;
using Application.Common.Models.Pagination;
using Application.Posts.Commands.VerifiedStudentLikePostOfVerifiedAccount;
using Application.Posts.Commands.VerifiedStudentUnlikePostOfVerifiedAccount;
using Application.Posts.Queries.GetLikedPost;
using Application.Posts.Queries.GetLikedPostsOfAccountWithPaging;
using Application.Posts.Queries.GetPost;
using Application.Posts.Queries.GetPostOfAccount;
using Application.Posts.Queries.GetPostsOfAccountWithPaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas.Company;

[Authorize(Role.Company)]
[Route("api/Company/[controller]")]
public class PostsController : ApiControllerBase
{
    [HttpGet("self/{postId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostDTO))]
    public async Task<IActionResult> GetPost(Guid postId)
    {
        var result = await Sender.Send(new GetPostOfAccountQuery
        {
            PostId = postId,
            IsAccountMustBeVerified = true,
            AccountId = AccountInfo!.Id
        });

        return Ok(result);
    }

    [HttpGet("self")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<PostDTO>))]
    public async Task<IActionResult> GetSelfPosts(
        [FromQuery] string? order,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetPostsOfAccountWithPagingQuery
        {
            AccountId = AccountInfo!.Id,
            PageNumber = pageNumber,
            PageSize = pageSize,
            OrderByExpression = order ?? "CreatedDate DESC",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }
}
