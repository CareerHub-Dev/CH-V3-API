using Application.Common.DTO;
using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries;

public record GetLanguageLevelsQuery : IRequest<IEnumerable<EnumDTO>>;

public class GetLanguageLevelsQueryHandler : IRequestHandler<GetLanguageLevelsQuery, IEnumerable<EnumDTO>>
{
    public Task<IEnumerable<EnumDTO>> Handle(GetLanguageLevelsQuery request, CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<LanguageLevel>()
            .Select(p => new EnumDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IEnumerable<EnumDTO>>(result);
    }
}