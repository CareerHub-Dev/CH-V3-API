using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries.GetTemplateLanguages;

public record GetTemplateLanguagesQuery : IRequest<IList<TemplateLanguageDTO>>;

public class GetTemplateLanguagesQueryHandler : IRequestHandler<GetTemplateLanguagesQuery, IList<TemplateLanguageDTO>>
{
    public GetTemplateLanguagesQueryHandler()
    {

    }

    public Task<IList<TemplateLanguageDTO>> Handle(GetTemplateLanguagesQuery request, CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<TemplateLanguage>()
            .Select(p => new TemplateLanguageDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IList<TemplateLanguageDTO>>(result);
    }
}