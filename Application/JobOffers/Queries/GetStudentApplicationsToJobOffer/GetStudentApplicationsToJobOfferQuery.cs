using Application.Common.DTO.JobOfferReviews;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffers.Queries.IsVerifiedStudentSubscribedToActiveJobOfferWithVerifiedCompany;

public record GetStudentApplicationsToJobOfferQuery
    : IRequest<IEnumerable<JobOfferReviewDTO>>
{
    public Guid StudentId { get; init; }
    public Guid JobOfferId { get; init; }
}

public class GetStudentApplicationsToJobOfferQueryHandler
    : IRequestHandler<GetStudentApplicationsToJobOfferQuery, IEnumerable<JobOfferReviewDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentApplicationsToJobOfferQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<JobOfferReviewDTO>> Handle(
        GetStudentApplicationsToJobOfferQuery request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Filter(isVerified: true)
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student is null)
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        return await _context.CVJobOffers
            .Include(x => x.CV)
            .ThenInclude(x => x!.Student)
            .Include(x => x!.JobOffer)
            .Where(x => x.CV!.Student!.Id == request.StudentId && x.JobOffer!.Id == request.JobOfferId)
            .Select(x => new JobOfferReviewDTO
            {
                Id = x.Id,
                Status = x.Status,
                Message = x.Message,
                Created = x.Created,
                CV = new ShortCVofReviewDTO
                {
                    Id = x.CV!.Id,
                    Title = x.CV.Title,
                    Created = x.CV.Created,
                    Modified = x.CV.Modified,
                },
                JobOffer = new ShortJobOfferOfReviewDTO
                {
                    Id = x.JobOffer!.Id,
                    Title = x.JobOffer.Title,
                    Image = x.JobOffer.Image,
                }
            })
            .ToListAsync(cancellationToken);
    }
}