using Application.Common.DTO;
using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries;

public record GetJobTypesQuery : IRequest<IList<EnumDTO>>;

public class GetJobTypesQueryHandler : IRequestHandler<GetJobTypesQuery, IList<EnumDTO>>
{
    public Task<IList<EnumDTO>> Handle(GetJobTypesQuery request, CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<JobType>()
            .Select(p => new EnumDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IList<EnumDTO>>(result);
    }
}