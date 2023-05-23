using Application.Common.DTO.CVs;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Queries.GetCVWord;

public record GetCVWordQuery(Guid CVId)
    : IRequest<MemoryStream>;

public class GetCVQueryHandler
    : IRequestHandler<GetCVWordQuery, MemoryStream>
{
    private readonly IApplicationDbContext _context;
    private readonly ICVWordGenerator _wordGenerator;

    public GetCVQueryHandler(
        IApplicationDbContext context,
        ICVWordGenerator wordGenerator)
    {
        _context = context;
        _wordGenerator = wordGenerator;
    }

    public async Task<MemoryStream> Handle(
        GetCVWordQuery request,
        CancellationToken cancellationToken)
    {
        var cv = await _context.CVs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.CVId);

        if (cv == null)
        {
            throw new NotFoundException(nameof(CV), request.CVId);
        }

        return _wordGenerator.GenerateDocument(cv);
    }
}