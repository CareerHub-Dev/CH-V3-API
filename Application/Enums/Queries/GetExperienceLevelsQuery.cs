using Application.Common.DTO;
using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries;

public record GetExperienceLevelsQuery : IRequest<IEnumerable<EnumDTO>>;

public class GetExperienceLevelsQueryHandler : IRequestHandler<GetExperienceLevelsQuery, IEnumerable<EnumDTO>>
{
    public Task<IEnumerable<EnumDTO>> Handle(GetExperienceLevelsQuery request, CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<ExperienceLevel>()
            .Select(p => new EnumDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IEnumerable<EnumDTO>>(result);
    }
}