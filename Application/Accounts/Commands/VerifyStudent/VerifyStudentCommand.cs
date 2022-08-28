using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.VerifyStudent;

public record VerifyStudentCommand : IRequest
{
    public string Token { get; init; } = string.Empty;
}

public class VerifyStudentCommandHandler : IRequestHandler<VerifyStudentCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifyStudentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(VerifyStudentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Students.SingleOrDefaultAsync(x => x.VerificationToken == request.Token, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Student), request.Token);
        }

        entity.VerificationToken = null;
        entity.Verified = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
