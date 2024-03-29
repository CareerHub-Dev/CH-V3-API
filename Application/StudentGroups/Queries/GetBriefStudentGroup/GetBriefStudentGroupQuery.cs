﻿using Application.Common.DTO.StudentGroups;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StudentGroups.Queries.GetBriefStudentGroup;

public record GetBriefStudentGroupQuery(Guid StudentGroupId)
    : IRequest<BriefStudentGroupDTO>;

public class GetBriefStudentGroupQueryHandler
    : IRequestHandler<GetBriefStudentGroupQuery, BriefStudentGroupDTO>
{
    private readonly IApplicationDbContext _context;

    public GetBriefStudentGroupQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BriefStudentGroupDTO> Handle(
        GetBriefStudentGroupQuery request,
        CancellationToken cancellationToken)
    {
        var studentGroup = await _context.StudentGroups
            .MapToBriefStudentGroupDTO()
            .FirstOrDefaultAsync(x => x.Id == request.StudentGroupId);

        if (studentGroup == null)
        {
            throw new NotFoundException(nameof(StudentGroup), request.StudentGroupId);
        }

        return studentGroup;
    }
}
