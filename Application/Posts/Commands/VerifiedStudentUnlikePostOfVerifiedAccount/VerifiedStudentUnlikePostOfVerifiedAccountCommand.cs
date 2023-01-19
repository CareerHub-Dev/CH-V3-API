using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Posts.Commands.VerifiedStudentUnlikePostOfVerifiedAccount;

public record VerifiedStudentUnlikePostOfVerifiedAccountCommand
    : IRequest
{
    public Guid StudentId { get; init; }
    public Guid PostId { get; init; }
}

public class VerifiedStudentUnlikePostOfVerifiedAccountCommandHandler
    : IRequestHandler<VerifiedStudentUnlikePostOfVerifiedAccountCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifiedStudentUnlikePostOfVerifiedAccountCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        VerifiedStudentUnlikePostOfVerifiedAccountCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Filter(isVerified: true)
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var post = await _context.Posts
           .Include(x => x.StudentsLiked)
           .Filter(isAccountVerified: true)
           .FirstOrDefaultAsync(x => x.Id == request.PostId);

        if (post == null)
        {
            throw new NotFoundException(nameof(Post), request.PostId);
        }

        var studentLiked = post.StudentsLiked
            .FirstOrDefault(x => x.Id == request.StudentId);

        if (studentLiked == null)
        {
            return Unit.Value;
        }

        post.StudentsLiked.Remove(studentLiked);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}