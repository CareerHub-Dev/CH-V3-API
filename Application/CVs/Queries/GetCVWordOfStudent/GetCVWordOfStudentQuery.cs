using Application.Common.DTO.CVs;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.CVs.Queries.GetCVWordOfStudent;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Queries.GetCVWordOfStudent;

public record GetCVWordOfStudentQuery
    : IRequest<MemoryStream>
{
    public Guid CVId { get; init; }

    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }
}

public class GetCVOfStudentQueryHandler
    : IRequestHandler<GetCVWordOfStudentQuery, MemoryStream>
{
    private readonly IApplicationDbContext _context;
    private readonly ICVWordGenerator _wordGenerator;

    public GetCVOfStudentQueryHandler(
        IApplicationDbContext context,
        ICVWordGenerator wordGenerator)
    {
        _context = context;
        _wordGenerator = wordGenerator;
    }

    public async Task<MemoryStream> Handle(
        GetCVWordOfStudentQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsStudentMustBeVerified)
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Company), request.StudentId);
        }

        var cv = await _context.CVs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.CVId && x.StudentId == request.StudentId);

        if (cv == null)
        {
            throw new NotFoundException(nameof(CV), request.CVId);
        }

        return _wordGenerator.GenerateDocument(cv);
    }
}