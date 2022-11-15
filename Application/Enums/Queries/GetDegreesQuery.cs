using Application.Common.DTO;
using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries;

public record GetDegreesQuery
    : IRequest<IEnumerable<EnumDTO>>;

public class GetDegreesQueryHandler
    : IRequestHandler<GetDegreesQuery, IEnumerable<EnumDTO>>
{
    public Task<IEnumerable<EnumDTO>> Handle(
        GetDegreesQuery request, 
        CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<Degree>()
            .Select(p => new EnumDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IEnumerable<EnumDTO>>(result);
    }
}