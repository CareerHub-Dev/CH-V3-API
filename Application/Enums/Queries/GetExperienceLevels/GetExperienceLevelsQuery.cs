using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries.GetExperienceLevels;

public record GetExperienceLevelsQuery : IRequest<IList<ExperienceLevelDTO>>;

public class GetExperienceLevelsQueryHandler : IRequestHandler<GetExperienceLevelsQuery, IList<ExperienceLevelDTO>>
{
    public GetExperienceLevelsQueryHandler()
    {

    }

    public Task<IList<ExperienceLevelDTO>> Handle(GetExperienceLevelsQuery request, CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<ExperienceLevel>()
            .Select(p => new ExperienceLevelDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IList<ExperienceLevelDTO>>(result);
    }
}