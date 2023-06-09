using Application.Common.DTO.Posts;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Queries.IsPostLikedByVerifiedStudent;

public record IsPostLikedByVerifiedStudentQuery
    : IRequest<bool>
{
    public required Guid StudentId { get; init; }
    public required Guid PostId { get; init; }
}

public class IsPostLikedByVerifiedStudentQueryHandler
    : IRequestHandler<IsPostLikedByVerifiedStudentQuery, bool>
{
    private readonly IApplicationDbContext _context;

    public IsPostLikedByVerifiedStudentQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        IsPostLikedByVerifiedStudentQuery request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .AsNoTracking()
            .Filter(isVerified: true)
            .Include(x => x.LikedPosts)
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student is null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return student.LikedPosts.Any(x => x.Id == request.PostId);
    }
}