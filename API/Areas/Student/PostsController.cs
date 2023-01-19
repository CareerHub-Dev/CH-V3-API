using API.Authorize;
using Application.Common.Enums;
using Application.Posts.Commands.VerifiedStudentLikePostOfVerifiedAccount;
using Application.Posts.Commands.VerifiedStudentUnlikePostOfVerifiedAccount;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Student;

[Authorize(Role.Student)]
[Route("api/Student/[controller]")]
public class PostsController : ApiControllerBase
{
    [HttpPost("{postId}/subscribe")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LikePost(Guid postId)
    {
        await Sender.Send(new VerifiedStudentLikePostOfVerifiedAccountCommand
        {
            StudentId = AccountInfo!.Id,
            PostId = postId
        });

        return NoContent();
    }

    [HttpDelete("{postId}/subscribe")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UnlikePost(Guid postId)
    {
        await Sender.Send(new VerifiedStudentUnlikePostOfVerifiedAccountCommand
        {
            StudentId = AccountInfo!.Id,
            PostId = postId
        });

        return NoContent();
    }
}
