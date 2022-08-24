using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Images.Queries.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Images.Queries;

public record GetImageQuery(Guid ImageId) : IRequest<ImageDTO>;

public class GetImageQueryHandler : IRequestHandler<GetImageQuery, ImageDTO>
{
    private readonly IApplicationDbContext _context;

    public GetImageQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ImageDTO> Handle(GetImageQuery request, CancellationToken cancellationToken)
    {
        var image = await _context.Images
            .AsNoTracking()
            .Where(x => x.Id == request.ImageId)
            .Select(x => new ImageDTO
            {
                Id = x.Id,
                ContentType = x.ContentType,
                Content = x.Content
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (image == null)
        {
            throw new NotFoundException(nameof(Image), request.ImageId);
        }

        return image;
    }
}
