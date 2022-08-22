using Application.Common.Entensions;
using Application.Common.Interfaces;
using Application.Common.Models.Search;
using Application.Common.Models.StudentGroup;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentGroups.Queries;

public record GetStudentGroupsQuery : IRequest<IEnumerable<StudentGroupDTO>>
{
    public SearchParameter? SearchParameter { get; init; }
}

public class GetStudentGroupsQueryHandler : IRequestHandler<GetStudentGroupsQuery, IEnumerable<StudentGroupDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentGroupsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StudentGroupDTO>> Handle(GetStudentGroupsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.StudentGroups
                .AsNoTracking();

        if (request.SearchParameter != null && request.SearchParameter.SearchTerm != null)
        {
            query = query.Search(request.SearchParameter.SearchTerm);
        }

        return await query
            .Select(x => new StudentGroupDTO
            {
                Id = x.Id,
                Name = x.Name,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                LastModified = x.LastModified,
                LastModifiedBy = x.LastModifiedBy,
            })
            .ToListAsync(cancellationToken);
    }
}