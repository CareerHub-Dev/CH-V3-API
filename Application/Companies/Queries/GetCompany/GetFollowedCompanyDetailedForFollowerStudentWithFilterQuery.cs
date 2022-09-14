using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Companies.Queries.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetCompany;

public record GetFollowedCompanyDetailedForFollowerStudentWithFilterQuery
    : IRequest<FollowedCompanyDetailedDTO>
{
    public Guid FollowerStudentId { get; init; }
    public bool? IsFollowerStudentMustBeVerified { get; init; }

    public Guid CompanyId { get; init; }
    public bool? IsCompanyMustBeVerified { get; init; }
}

public class GetFollowedCompanyDetailedForFollowerStudentWithFilterQueryHandler
    : IRequestHandler<GetFollowedCompanyDetailedForFollowerStudentWithFilterQuery, FollowedCompanyDetailedDTO>
{
    private readonly IApplicationDbContext _context;

    public GetFollowedCompanyDetailedForFollowerStudentWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FollowedCompanyDetailedDTO> Handle(GetFollowedCompanyDetailedForFollowerStudentWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsFollowerStudentMustBeVerified)
            .AnyAsync(x => x.Id == request.FollowerStudentId))
        {
            throw new NotFoundException(nameof(Student), request.FollowerStudentId);
        }

        var company = await _context.Companies
            .AsNoTracking()
            .Where(x => x.Id == request.CompanyId)
            .Filter(isVerified: request.IsCompanyMustBeVerified)
            .Select(x => new FollowedCompanyDetailedDTO
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                LogoId = x.LogoId,
                BannerId = x.BannerId,
                Motto = x.Motto,
                Description = x.Description,
                IsFollowed = x.SubscribedStudents.Any(x => x.Id == request.FollowerStudentId)
            })
            .FirstOrDefaultAsync();

        if (company == null)
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return company;
    }
}