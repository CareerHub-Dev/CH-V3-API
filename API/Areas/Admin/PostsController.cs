using API.Authorize;
using Application.Common.DTO.Posts;
using Application.Common.Enums;
using Application.Common.Models.Pagination;
using Application.Posts.Commands.CreatePost;
using Application.Posts.Commands.DeletePost;
using Application.Posts.Commands.UpdatePost;
using Application.Posts.Queries.GetPostsWithPaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas.Admin;

[Authorize(Role.Admin, Role.SuperAdmin)]
[Route("api/Admin/[controller]")]
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
            PageNumber = pageNumber,
            PageSize = pageSize,
            OrderByExpression = order ?? "CreatedDate",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

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
    public async Task<IActionResult> UpdatePost(Guid postId, [FromForm] UpdatePostCommand commnd)
    {
        if (postId != commnd.PostId)
        {
            return BadRequest();
        }

        await Sender.Send(commnd);

        return NoContent();
    }
}
