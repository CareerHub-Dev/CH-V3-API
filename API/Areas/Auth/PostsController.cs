using API.Authorize;
using API.DTO.Requests.Posts;
using Application.Common.DTO.Posts;
using Application.Common.Models.Pagination;
using Application.Posts.Commands.CreatePost;
using Application.Posts.Commands.DeletePostOfAccount;
using Application.Posts.Commands.UpdatePostOfAccount;
using Application.Posts.Queries.GetPostsOfAccountWithPaging;
using Application.Posts.Queries.GetPostsWithPaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class PostsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<PostDTO>))]
    public async Task<IActionResult> GetPosts(
        [FromQuery] string? order,
        [FromQuery] Guid? accountId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetPostsWithPagingQuery
        {
            AccountId = accountId,
            IsAccountMustBeVerified = true,
            PageNumber = pageNumber,
            PageSize = pageSize,
            OrderByExpression = order ?? "CreatedDate",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("self")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<PostDTO>))]
    public async Task<IActionResult> GetSelfPosts(
        [FromQuery] string? order,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetPostsOfAccountWithPaging
        {
            AccountId = AccountInfo!.Id,
            PageNumber = pageNumber,
            PageSize = pageSize,
            OrderByExpression = order ?? "CreatedDate",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpPost("self")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> CreatePostForSelfAccount([FromForm] CreateOwnPostRequest request)
    {
        var result = await Sender.Send(new CreatePostCommand
        {
            AccountId = AccountInfo!.Id,
            Text = request.Text,
            Images = request.Images,
        });

        return Ok(result);
    }

    [HttpDelete("self/{postId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeletePostOfSelfAccount(Guid postId)
    {
        await Sender.Send(new DeletePostOfAccountCommand(postId, AccountInfo!.Id));

        return NoContent();
    }

    [HttpPut("self/{postId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePostOfSelfAccount(Guid postId, [FromForm] UpdateOwnPostRequest request)
    {
        if (postId != request.PostId)
        {
            return BadRequest();
        }

        await Sender.Send(new UpdatePostOfAccountCommand
        {
            PostId = request.PostId,
            AccountId = AccountInfo!.Id,
            Text = request.Text,
        });

        return NoContent();
    }
}
