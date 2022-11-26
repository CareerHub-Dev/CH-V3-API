using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Commands.DeleteCV;

public record DeleteCVCommand(Guid CVId)
    : IRequest;

public class DeleteCVCommandHandler
    : IRequestHandler<DeleteCVCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCVCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteCVCommand request,
        CancellationToken cancellationToken)
    {
        var cv = await _context.CVs
            .FirstOrDefaultAsync(x => x.Id == request.CVId);

        if (cv == null)
        {
            throw new NotFoundException(nameof(CV), request.CVId);
        }

        _context.CVs.Remove(cv);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}