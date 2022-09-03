using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Tags.Commands.CreateTag;

public record CreateTagCommand : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;
    public bool IsAccepted { get; init; }
}

public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = new Tag
        {
            Name = request.Name,
            IsAccepted = request.IsAccepted,
        };

        await _context.Tags.AddAsync(tag);

        await _context.SaveChangesAsync();

        return tag.Id;
    }
}
