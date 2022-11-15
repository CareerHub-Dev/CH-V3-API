using Application.Common.DTO;
using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries;

public record GetWorkFormatsQuery
    : IRequest<IEnumerable<EnumDTO>>;

public class GetWorkFormatsQueryHandler
    : IRequestHandler<GetWorkFormatsQuery, IEnumerable<EnumDTO>>
{
    public Task<IEnumerable<EnumDTO>> Handle(
        GetWorkFormatsQuery request, 
        CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<WorkFormat>()
            .Select(p => new EnumDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IEnumerable<EnumDTO>>(result);
    }
}