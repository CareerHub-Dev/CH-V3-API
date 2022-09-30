using Application.Common.DTO;
using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries;

public record GetDegreesQuery : IRequest<IList<EnumDTO>>;

public class GetDegreesQueryHandler : IRequestHandler<GetDegreesQuery, IList<EnumDTO>>
{
    public Task<IList<EnumDTO>> Handle(GetDegreesQuery request, CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<Degree>()
            .Select(p => new EnumDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IList<EnumDTO>>(result);
    }
}