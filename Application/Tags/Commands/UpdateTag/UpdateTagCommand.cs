using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Commands.UpdateTag;

public record UpdateTagCommand
    : IRequest
{
    public Guid TagId { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool IsAccepted { get; init; }
}

public class UpdateTagCommandHandler
    : IRequestHandler<UpdateTagCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTagCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        UpdateTagCommand request,
        CancellationToken cancellationToken)
    {
        var tag = await _context.Tags
            .Where(x => x.Id == request.TagId)
            .FirstOrDefaultAsync();

        if (tag == null)
        {
            throw new NotFoundException(nameof(Tag), request.TagId);
        }

        tag.Name = request.Name;
        tag.IsAccepted = request.IsAccepted;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
