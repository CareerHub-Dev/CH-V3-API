using Application.Common.DTO;
using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries;

public record GetTemplateLanguagesQuery : IRequest<IList<EnumDTO>>;

public class GetTemplateLanguagesQueryHandler : IRequestHandler<GetTemplateLanguagesQuery, IList<EnumDTO>>
{
    public GetTemplateLanguagesQueryHandler() { }

    public Task<IList<EnumDTO>> Handle(GetTemplateLanguagesQuery request, CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<TemplateLanguage>()
            .Select(p => new EnumDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IList<EnumDTO>>(result);
    }
}