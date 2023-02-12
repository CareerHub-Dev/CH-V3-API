using API.Authorize;
using Application.Common.DTO.Posts;
using Application.Common.Enums;
using Application.Common.Models.Pagination;
using Application.Posts.Commands.CreatePost;
using Application.Posts.Commands.DeletePost;
using Application.Posts.Commands.UpdatePost;
using Application.Posts.Queries.GetAdmininstrationPostsWithPaging;
using Application.Posts.Queries.GetPost;
using Application.Posts.Queries.GetPostsOfAccountWithPaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas.Admin;

[Authorize(Role.Admin, Role.SuperAdmin)]
[Route("api/Admin/[controller]")]
public class PostsController : ApiControllerBase
{
    [HttpGet("of-account/{accountId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<PostDTO>))]
    public async Task<IActionResult> GetPostsOfAccount(
        Guid accountId,
        [FromQuery] string? order,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetPostsOfAccountWithPagingQuery
        {
            AccountId = accountId,
            IsAccountMustBeVerified = true,
            PageNumber = pageNumber,
            PageSize = pageSize,
            OrderByExpression = order ?? "CreatedDate DESC",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("admininstration")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<PostDTO>))]
    public async Task<IActionResult> GetPostsOfadmininstration(
        [FromQuery] string? order,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetAdmininstrationPostsWithPagingQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            OrderByExpression = order ?? "CreatedDate DESC",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{postId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostDTO))]
    public async Task<IActionResult> GetPost(Guid postId)
    {
        var result = await Sender.Send(new GetPostQuery
        {
            PostId = postId
        });

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreatePost([FromForm] CreatePostCommand command)
    {
        var result = await Sender.Send(command);

        return Ok(result);
    }

    [HttpDelete("{postId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePost(Guid postId)
    {
        await Sender.Send(new DeletePostCommand(postId));

        return NoContent();
    }

    [HttpPut("{postId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePost(Guid postId, UpdatePostCommand commnd)
    {
        if (postId != commnd.PostId)
        {
            return BadRequest();
        }

        await Sender.Send(commnd);

        return NoContent();
    }
}
