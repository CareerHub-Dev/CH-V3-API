using Application.Common.DTO;
using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries;

public record GetTemplateLanguagesQuery
    : IRequest<IEnumerable<EnumDTO>>;

public class GetTemplateLanguagesQueryHandler
    : IRequestHandler<GetTemplateLanguagesQuery, IEnumerable<EnumDTO>>
{
    public Task<IEnumerable<EnumDTO>> Handle(
        GetTemplateLanguagesQuery request, 
        CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<TemplateLanguage>()
            .Select(p => new EnumDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IEnumerable<EnumDTO>>(result);
    }
}