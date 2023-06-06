using Application.Common.DTO.CVs;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Queries.GetCVOfStudent;

public record GetAppliedCVQuery
    : IRequest<CVDTO>
{
    public Guid CVId { get; init; }
}

public class GetAppliedCVQueryHandler
    : IRequestHandler<GetAppliedCVQuery, CVDTO>
{
    private readonly IApplicationDbContext _context;

    public GetAppliedCVQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CVDTO> Handle(
        GetAppliedCVQuery request,
        CancellationToken cancellationToken)
    {
        // TODO: also validate that company is the owner of the job offer
        var cv = await _context.CVs
            .MapToCVDTO()
            .FirstOrDefaultAsync(x => x.Id == request.CVId);

        if (cv is null)
        {
            throw new NotFoundException(nameof(CV), request.CVId);
        }

        return cv;
    }
}