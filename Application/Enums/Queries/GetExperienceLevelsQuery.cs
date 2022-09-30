using Application.Common.DTO;
using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries;

public record GetExperienceLevelsQuery : IRequest<IList<EnumDTO>>;

public class GetExperienceLevelsQueryHandler : IRequestHandler<GetExperienceLevelsQuery, IList<EnumDTO>>
{
    public GetExperienceLevelsQueryHandler() { }

    public Task<IList<EnumDTO>> Handle(GetExperienceLevelsQuery request, CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<ExperienceLevel>()
            .Select(p => new EnumDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IList<EnumDTO>>(result);
    }
}