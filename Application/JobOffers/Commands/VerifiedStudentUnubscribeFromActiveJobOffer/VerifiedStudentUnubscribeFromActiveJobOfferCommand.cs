using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Commands.VerifiedStudentUnubscribeFromActiveJobOffer;

public record VerifiedStudentUnubscribeFromActiveJobOfferCommand : IRequest
{
    public Guid StudentId { get; init; }
    public Guid JobOfferId { get; init; }
}

public class VerifiedStudentUnubscribeFromActiveJobOfferCommandHandler : IRequestHandler<VerifiedStudentUnubscribeFromActiveJobOfferCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifiedStudentUnubscribeFromActiveJobOfferCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(VerifiedStudentUnubscribeFromActiveJobOfferCommand request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Filter(isVerified: true)
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var company = await _context.JobOffers
            .Include(x => x.SubscribedStudents)
            .Filter(isActive: true)
            .FirstOrDefaultAsync(x => x.Id == request.JobOfferId);

        if (company == null)
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        var subscribedStudent = company.SubscribedStudents.FirstOrDefault(x => x.Id == request.StudentId);

        if (subscribedStudent == null)
        {
            return Unit.Value;
        }

        company.SubscribedStudents.Remove(subscribedStudent);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}