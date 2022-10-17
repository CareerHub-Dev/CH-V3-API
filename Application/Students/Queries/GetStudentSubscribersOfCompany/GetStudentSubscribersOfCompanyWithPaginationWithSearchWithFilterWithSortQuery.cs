using Application.Common.DTO.StudentGroups;
using Application.Common.DTO.Students;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.Pagination;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetStudentSubscribersOfCompany;

public record GetStudentSubscribersOfCompanyWithPaginationWithSearchWithFilterWithSortQuery
    : IRequest<PaginatedList<StudentDTO>>
{
    public Guid CompanyId { get; init; }
    public bool? IsCompanyMustBeVerified { get; init; }
    public ActivationStatus? CompanyMustHaveActivationStatus { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string SearchTerm { get; init; } = string.Empty;

    public bool? IsStudentMustBeVerified { get; init; }
    public Guid? WithoutStudentId { get; init; }
    public List<Guid>? StudentGroupIds { get; init; }
    public ActivationStatus? StudentMustHaveActivationStatus { get; init; }

    public string OrderByExpression { get; init; } = string.Empty;
}

public class GetStudentSubscribersOfCompanyWithPaginationWithSearchWithFilterWithSortQueryHandler
    : IRequestHandler<GetStudentSubscribersOfCompanyWithPaginationWithSearchWithFilterWithSortQuery, PaginatedList<StudentDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentSubscribersOfCompanyWithPaginationWithSearchWithFilterWithSortQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<StudentDTO>> Handle(
        GetStudentSubscribersOfCompanyWithPaginationWithSearchWithFilterWithSortQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Companies
            .Filter(
                isVerified: request.IsCompanyMustBeVerified,
                activationStatus: request.CompanyMustHaveActivationStatus
            )
            .AnyAsync(x => x.Id == request.CompanyId))
        {
            throw new NotFoundException(nameof(Company), request.CompanyId);
        }

        return await _context.Students
            .AsNoTracking()
            .Where(x => x.CompanySubscriptions.Any(x => x.Id == request.CompanyId))
            .Filter(
                withoutStudentId: request.WithoutStudentId,
                isVerified: request.IsStudentMustBeVerified,
                studentGroupIds: request.StudentGroupIds,
                activationStatus: request.StudentMustHaveActivationStatus
            )
            .Search(request.SearchTerm)
            .Select(x => new StudentDTO
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Photo = x.Photo,
                Phone = x.Phone,
                BirthDate = x.BirthDate,
                StudentGroup = new BriefStudentGroupDTO { Id = x.StudentGroup!.Id, Name = x.StudentGroup.Name },
                Verified = x.Verified,
                PasswordReset = x.PasswordReset,
                ActivationStatus = x.ActivationStatus
            })
            .OrderByExpression(request.OrderByExpression)
            .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}