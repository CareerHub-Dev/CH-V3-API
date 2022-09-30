using Application.Common.DTO;
using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries;

public record GetWorkFormatsQuery : IRequest<IList<EnumDTO>>;

public class GetWorkFormatsQueryHandler : IRequestHandler<GetWorkFormatsQuery, IList<EnumDTO>>
{
    public Task<IList<EnumDTO>> Handle(GetWorkFormatsQuery request, CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<WorkFormat>()
            .Select(p => new EnumDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IList<EnumDTO>>(result);
    }
}