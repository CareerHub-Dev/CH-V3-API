﻿using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Commands.UpdateStudent;

public record UpdateStudentCommand : IRequest
{
    public Guid StudentId { get; set; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public DateTime? BirthDate { get; init; }
    public Guid StudentGroupId { get; init; }
}

public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateStudentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Students
            .FirstOrDefaultAsync(x => x.Id == request.StudentId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.Phone = request.Phone;
        entity.StudentGroupId = request.StudentGroupId;
        entity.BirthDate = request.BirthDate;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}