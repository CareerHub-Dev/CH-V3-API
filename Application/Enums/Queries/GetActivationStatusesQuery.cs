using Application.Common.DTO;
using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries;

public record GetActivationStatusesQuery : IRequest<IEnumerable<EnumDTO>>;

public class GetActivationStatusesQueryHandler : IRequestHandler<GetActivationStatusesQuery, IEnumerable<EnumDTO>>
{
    public Task<IEnumerable<EnumDTO>> Handle(GetActivationStatusesQuery request, CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<ActivationStatus>()
            .Select(p => new EnumDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IEnumerable<EnumDTO>>(result);
    }
}