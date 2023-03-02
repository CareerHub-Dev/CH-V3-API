using Application.Common.DTO.JobOfferReviews;
using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOfferReviews.Queries.GetJobOfferReviewOfStudent;

public class GetJobOfferReviewOfStudentQuery : IRequest<JobOfferReviewDTO>
{
    public Guid ReviewId { get; set; }
    public Guid StudentId { get; init; }
    public bool? IsStudentMustBeVerified { get; init; }
}

public class GetJobOfferReviewOfStudentQueryHandler
    : IRequestHandler<GetJobOfferReviewOfStudentQuery, JobOfferReviewDTO>
{
    private readonly IApplicationDbContext _context;

    public GetJobOfferReviewOfStudentQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JobOfferReviewDTO> Handle(
        GetJobOfferReviewOfStudentQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _context.Students
            .Filter(isVerified: request.IsStudentMustBeVerified)
            .AnyAsync(x => x.Id == request.StudentId))
        {
            throw new NotFoundException(nameof(Student), request.StudentId);
        }

        var result = await _context.CVJobOffers
            .Where(x => x.CV!.StudentId == request.StudentId && x.Id == request.ReviewId)
            .MapToJobOfferReviewDTO()
            .FirstOrDefaultAsync();

        if (result == null)
        {
            throw new NotFoundException(nameof(CVJobOffer), request.ReviewId);
        }

        return result;
    }
}