using Application.Common.DTO.JobOfferReviews;
using Application.Common.DTO.Students;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOfferReviews.Queries.GetReviewDetailsAsCompany;


public class GetReviewDetailsAsCompanyQuery : IRequest<DetailedJobOfferReviewDTO>
{ 
    public required Guid ReviewId { get; init; }
    public required Guid CompanyId { get; init; }
}

public class GetReviewDetailsAsCompanyQueryHandler
    : IRequestHandler<GetReviewDetailsAsCompanyQuery, DetailedJobOfferReviewDTO>
{
    private readonly IApplicationDbContext _context;

    public GetReviewDetailsAsCompanyQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DetailedJobOfferReviewDTO> Handle(
        GetReviewDetailsAsCompanyQuery request,
        CancellationToken cancellationToken)
    {
        var review = await _context.CVJobOffers
            .Include(x => x.JobOffer)
            .Include(x => x.CV)
            .ThenInclude(x => x!.Student)
            .ThenInclude(x => x!.StudentGroup)
            .FirstOrDefaultAsync(x => x.Id == request.ReviewId, cancellationToken);

        if (review is null)
        {
            throw new NotFoundException("Review", request.ReviewId);
        }

        var reviewCannotBeAccessed = review.JobOffer?.CompanyId != request.CompanyId;

        if (reviewCannotBeAccessed) 
        {
            throw new AccessViolationException("Review cannot be accessed");
        }

        var cv = review.CV!;
        var jobOffer = review.JobOffer!;
        var student = review.CV!.Student!;

        return new DetailedJobOfferReviewDTO() 
        {
            Id = review.Id,
            Status = review.Status,
            Message = review.Message,
            Created = review.Created,
            CV = new ShortCVofReviewDTO() { Created =  cv.Created, Id = cv.Id, Modified = cv.Modified, Title = cv.Title },
            JobOffer = new ShortJobOfferOfReviewDTO() { Id = jobOffer.Id, Title = jobOffer.Title, Image = jobOffer.Image },
            Student = DetailedStudentDTO.FromEntity(student),
        };
    }
}
