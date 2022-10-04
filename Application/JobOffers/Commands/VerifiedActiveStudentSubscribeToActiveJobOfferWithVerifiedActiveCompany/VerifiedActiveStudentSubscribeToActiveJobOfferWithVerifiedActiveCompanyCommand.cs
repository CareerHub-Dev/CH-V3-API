﻿using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Commands.VerifiedActiveStudentSubscribeToActiveJobOfferWithVerifiedActiveCompany;

public record VerifiedActiveStudentSubscribeToActiveJobOfferWithVerifiedActiveCompanyCommand : IRequest
{
    public Guid StudentId { get; init; }
    public Guid JobOfferId { get; init; }
}

public class VerifiedActiveStudentSubscribeToActiveJobOfferWithVerifiedActiveCompanyCommandHandler
    : IRequestHandler<VerifiedActiveStudentSubscribeToActiveJobOfferWithVerifiedActiveCompanyCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifiedActiveStudentSubscribeToActiveJobOfferWithVerifiedActiveCompanyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(VerifiedActiveStudentSubscribeToActiveJobOfferWithVerifiedActiveCompanyCommand request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Filter(
                isVerified: true, 
                activationStatus: ActivationStatus.Active
            )
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var joboffer = await _context.JobOffers
            .Include(x => x.SubscribedStudents)
            .Filter(
                isActive: true,
                isCompanyVerified: true,
                companyActivationStatus: ActivationStatus.Active
            )
            .FirstOrDefaultAsync(x => x.Id == request.JobOfferId);

        if (joboffer == null)
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        if (joboffer.SubscribedStudents.Any(x => x.Id == request.StudentId))
        {
            return Unit.Value;
        }

        joboffer.SubscribedStudents.Add(student);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}