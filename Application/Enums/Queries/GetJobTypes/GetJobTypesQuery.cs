using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries.GetJobTypes;

public record GetJobTypesQuery : IRequest<IList<JobTypeDTO>>;

public class GetJobTypesQueryHandler : IRequestHandler<GetJobTypesQuery, IList<JobTypeDTO>>
{
    public GetJobTypesQueryHandler()
    {

    }

    public Task<IList<JobTypeDTO>> Handle(GetJobTypesQuery request, CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<JobType>()
            .Select(p => new JobTypeDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IList<JobTypeDTO>>(result);
    }
}