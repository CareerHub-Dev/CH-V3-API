using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries.GetDegrees;

public record GetDegreesQuery : IRequest<IList<DegreeDTO>>;

public class GetDegreesQueryHandler : IRequestHandler<GetDegreesQuery, IList<DegreeDTO>>
{
    public GetDegreesQueryHandler()
    {

    }

    public Task<IList<DegreeDTO>> Handle(GetDegreesQuery request, CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<Degree>()
            .Select(p => new DegreeDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IList<DegreeDTO>>(result);
    }
}