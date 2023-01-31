using Application.Common.DTO.Posts;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Application.Posts.Queries.GetLikedPostsOfAccountWithPaging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Queries.GetPostsOfFollowedAccountsForStudentWithPaging;

public class GetPostsOfFollowedAccountsForStudentWithPagingQuery
    : IRequest<PaginatedList<LikedPostDTO>>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }

    public bool? IsAccountMustBeVerified { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetPostsOfFollowedAccountsForStudentWithPagingQueryHandler
    : IRequestHandler<GetPostsOfFollowedAccountsForStudentWithPagingQuery, PaginatedList<LikedPostDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetPostsOfFollowedAccountsForStudentWithPagingQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<LikedPostDTO>> Handle(
        GetPostsOfFollowedAccountsForStudentWithPagingQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsStudentMustBeVerified)
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var followedCompanies = await _context.Students
            .Where(x => x.Id == request.StudentId)
            .SelectMany(x => x.CompanySubscriptions)
            .Select(x => x.Id)
            .ToListAsync();

        var followedStudents = await _context.Students
            .Where(x => x.Id == request.StudentId)
            .SelectMany(x => x.StudentSubscriptions)
            .Select(x => x.SubscriptionTargetId)
            .ToListAsync();

        var administration = await _context.Admins
            .Select(x => x.Id)
            .ToListAsync();

        var all = followedCompanies.Concat(followedStudents).Concat(administration).ToList();

        return await _context.Posts
            .Filter(isAccountVerified: request.IsAccountMustBeVerified, accountIds: all)
            .MapToLikedPostDTO(request.StudentId)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}