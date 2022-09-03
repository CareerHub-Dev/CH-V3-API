using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Commands.DeleteTag;

public record DeleteTagCommand(Guid TagId) : IRequest;

public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.TagId);

        if (tag == null)
        {
            throw new NotFoundException(nameof(Tag), request.TagId);
        }

        _context.Tags.Remove(tag);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
