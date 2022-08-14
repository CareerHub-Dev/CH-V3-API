using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Images.Queries.GetImage;

public record GetImageQuery(Guid Id) : IRequest<ImageDTO>;

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
            .Where(x => x.Id == request.Id)
            .Select(x => new ImageDTO
            {
                Id = x.Id,
                FileName = x.FileName,
                Content = x.Content
            })
            .FirstOrDefaultAsync();

        if (image == null)
        {
            throw new NotFoundException("image", request.Id);
        }

        return image;
    }
}
