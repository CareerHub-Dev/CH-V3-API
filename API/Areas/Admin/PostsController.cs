using API.Authorize;
using Application.Common.Enums;
using Application.Posts.Commands.CreatePost;
using Application.Posts.Commands.DeletePost;
using Application.Posts.Commands.UpdatePost;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Admin;

[Authorize(Role.Admin, Role.SuperAdmin)]
[Route("api/Admin/[controller]")]
public class PostsController : ApiControllerBase
{
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
