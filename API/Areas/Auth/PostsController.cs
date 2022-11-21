using API.Authorize;
using API.DTO.Requests.Posts;
using Application.Posts.Commands.CreatePost;
using Application.Posts.Commands.DeletePostOfAccount;
using Application.Posts.Commands.UpdatePostOfAccount;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Auth;

[Authorize]
[Route("api/Auth/[controller]")]
public class PostsController : ApiControllerBase
{
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
