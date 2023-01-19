using Application.Common.DTO;
using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries;

public record GetReviewsQuery
    : IRequest<IEnumerable<EnumDTO>>;

public class GetReviewsQueryHandler
    : IRequestHandler<GetReviewsQuery, IEnumerable<EnumDTO>>
{
    public Task<IEnumerable<EnumDTO>> Handle(
        GetReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<Review>()
            .Select(p => new EnumDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IEnumerable<EnumDTO>>(result);
    }
}