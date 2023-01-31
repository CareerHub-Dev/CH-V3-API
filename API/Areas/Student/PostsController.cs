using API.Authorize;
using Application.Common.DTO.Posts;
using Application.Common.Enums;
using Application.Common.Models.Pagination;
using Application.Posts.Commands.VerifiedStudentLikePostOfVerifiedAccount;
using Application.Posts.Commands.VerifiedStudentUnlikePostOfVerifiedAccount;
using Application.Posts.Queries.GetLikedPost;
using Application.Posts.Queries.GetLikedPostsOfAccountWithPaging;
using Application.Posts.Queries.GetPostsOfFollowedAccountsForStudentWithPaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas.Student;

[Authorize(Role.Student)]
[Route("api/Student/[controller]")]
public class PostsController : ApiControllerBase
{
    [HttpGet("followed-accounts")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<LikedPostDTO>))]
    public async Task<IActionResult> GetLikedPostsOfFollowedAccounts(
        Guid accountId,
        [FromQuery] string? order,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetPostsOfFollowedAccountsForStudentWithPagingQuery
        {
            StudentId = accountId,
            IsStudentMustBeVerified = true,
            IsAccountMustBeVerified = true,
            PageNumber = pageNumber,
            PageSize = pageSize,
            OrderByExpression = order ?? "CreatedDate",
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("of-account/{accountId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<LikedPostDTO>))]
    public async Task<IActionResult> GetLikedPostsOfAccount(
        Guid accountId,
        [FromQuery] string? order,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetLikedPostsOfAccountWithPagingQuery
        {
            AccountId = accountId,
            IsAccountMustBeVerified = true,
            PageNumber = pageNumber,
            PageSize = pageSize,
            OrderByExpression = order ?? "CreatedDate",
            LikerStudentId = AccountInfo!.Id
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{postId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LikedPostDTO))]
    public async Task<IActionResult> GetLikedPost(Guid postId)
    {
        var result = await Sender.Send(new GetLikedPostQuery
        {
            PostId = postId,
            IsAccountMustBeVerified = true,
            LikerStudentId = AccountInfo!.Id
        });

        return Ok(result);
    }

    [HttpGet("self")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<LikedPostDTO>))]
    public async Task<IActionResult> GetSelfLikedPosts(
        [FromQuery] string? order,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetLikedPostsOfAccountWithPagingQuery
        {
            AccountId = AccountInfo!.Id,
            PageNumber = pageNumber,
            PageSize = pageSize,
            OrderByExpression = order ?? "CreatedDate",
            LikerStudentId = AccountInfo!.Id
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpPost("{postId}/like")]
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

    [HttpDelete("{postId}/unlike")]
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
