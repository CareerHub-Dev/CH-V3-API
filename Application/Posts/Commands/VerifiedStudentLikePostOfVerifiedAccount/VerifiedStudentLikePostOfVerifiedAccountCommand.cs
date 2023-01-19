using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Commands.VerifiedStudentLikePostOfVerifiedAccount;

public record VerifiedStudentLikePostOfVerifiedAccountCommand
    : IRequest
{
    public Guid StudentId { get; init; }
    public Guid PostId { get; init; }
}

public class VerifiedStudentLikePostOfVerifiedAccountCommandHandler
    : IRequestHandler<VerifiedStudentLikePostOfVerifiedAccountCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifiedStudentLikePostOfVerifiedAccountCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        VerifiedStudentLikePostOfVerifiedAccountCommand request,
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

        if (post.StudentsLiked.Any(x => x.Id == request.StudentId))
        {
            return Unit.Value;
        }

        post.StudentsLiked.Add(student);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}