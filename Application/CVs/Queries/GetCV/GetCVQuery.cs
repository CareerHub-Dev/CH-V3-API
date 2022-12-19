using Application.Common.DTO.CVs;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Queries.GetCV;

public record GetCVQuery(Guid CVId)
    : IRequest<CVDTO>;

public class GetCVQueryHandler
    : IRequestHandler<GetCVQuery, CVDTO>
{
    private readonly IApplicationDbContext _context;

    public GetCVQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CVDTO> Handle(
        GetCVQuery request,
        CancellationToken cancellationToken)
    {
        var cv = await _context.CVs
            .MapToCVDTO()
            .FirstOrDefaultAsync(x => x.Id == request.CVId);

        if (cv == null)
        {
            throw new NotFoundException(nameof(CV), request.CVId);
        }

        return cv;
    }
}