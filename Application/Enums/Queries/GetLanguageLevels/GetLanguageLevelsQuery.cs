using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries.GetLanguageLevels;

public record GetLanguageLevelsQuery : IRequest<IList<LanguageLevelDTO>>;

public class GetLanguageLevelsQueryHandler : IRequestHandler<GetLanguageLevelsQuery, IList<LanguageLevelDTO>>
{
    public GetLanguageLevelsQueryHandler()
    {

    }

    public Task<IList<LanguageLevelDTO>> Handle(GetLanguageLevelsQuery request, CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<LanguageLevel>()
            .Select(p => new LanguageLevelDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IList<LanguageLevelDTO>>(result);
    }
}