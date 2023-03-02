using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Commands.SendCVOfStudentForJobOffer;

public record SendCVOfStudentForJobOfferCommand
    : IRequest
{
    public Guid CVId { get; init; }
    public Guid StudentId { get; init; }

    public Guid JobOfferId { get; init; }
    public bool? IsJobOfferMustBeActive { get; init; }
    public bool? IsCompanyOfJobOfferMustBeVerified { get; init; }
}

public class SendCVOfStudentForJobOfferCommandHandler
    : IRequestHandler<SendCVOfStudentForJobOfferCommand>
{
    private readonly IApplicationDbContext _context;

    public SendCVOfStudentForJobOfferCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        SendCVOfStudentForJobOfferCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var cv = await _context.CVs.FirstOrDefaultAsync(x => x.Id == request.CVId && x.StudentId == request.StudentId);

        if (cv == null)
        {
            throw new NotFoundException(nameof(CV), request.CVId);
        }

        var joboffer = await _context.JobOffers
            .Filter(
                isActive: true,
                isCompanyVerified: true
            )
            .FirstOrDefaultAsync(x => x.Id == request.JobOfferId);

        if (joboffer == null)
        {
            throw new NotFoundException(nameof(JobOffer), request.JobOfferId);
        }

        var cvJobOffer = new CVJobOffer
        {
            CVId = request.CVId,
            JobOfferId = request.JobOfferId,
        };

        _context.CVJobOffers.Add(cvJobOffer);
        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}