using Application.Common.Entensions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CVs.Commands.SendCVOfStudentForJobOffer;

public record UpdateCVReviewCommand
    : IRequest
{
    public required Guid CVReviewId { get; init; }
    public required Review Status { get; init; }
    public required string Message { get; init; }
}

public class UpdateCVReviewCommandHandler
    : IRequestHandler<UpdateCVReviewCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCVReviewCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        UpdateCVReviewCommand request,
        CancellationToken cancellationToken)
    {
        var review = await _context.CVJobOffers
            .FirstOrDefaultAsync(x => x.Id == request.CVReviewId);

        if (review is null)
        {
            throw new NotFoundException(nameof(CVJobOffer), request.CVReviewId);
        }

        review.Status = request.Status;
        review.Message = request.Message;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}