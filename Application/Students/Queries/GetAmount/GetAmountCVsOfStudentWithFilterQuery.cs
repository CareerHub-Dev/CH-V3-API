﻿using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries.GetAmount;

public class GetAmountCVsOfStudentWithFilterQuery : IRequest<int>
{
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }
    public ActivationStatus? StudentMustHaveActivationStatus { get; init; }
}

public class GetAmountCVsOfStudentWithFilterQueryHandler
    : IRequestHandler<GetAmountCVsOfStudentWithFilterQuery, int>
{
    private readonly IApplicationDbContext _context;

    public GetAmountCVsOfStudentWithFilterQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(GetAmountCVsOfStudentWithFilterQuery request, CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(
                isVerified: request.IsStudentMustBeVerified,
                activationStatus: request.StudentMustHaveActivationStatus
            )
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return await _context.Students
            .Where(x => x.Id == request.StudentId)
            .SelectMany(x => x.CVs)
            .CountAsync();
    }
}