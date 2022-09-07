using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries.GetWorkFormats;

public record GetWorkFormatsQuery : IRequest<IList<WorkFormatDTO>>;

public class GetWorkFormatsQueryHandler : IRequestHandler<GetWorkFormatsQuery, IList<WorkFormatDTO>>
{
    public GetWorkFormatsQueryHandler()
    {

    }

    public Task<IList<WorkFormatDTO>> Handle(GetWorkFormatsQuery request, CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<WorkFormat>()
            .Select(p => new WorkFormatDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IList<WorkFormatDTO>>(result);
    }
}