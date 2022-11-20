using Application.Common.DTO;
using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries;

public record GetJobTypesQuery
    : IRequest<IEnumerable<EnumDTO>>;

public class GetJobTypesQueryHandler
    : IRequestHandler<GetJobTypesQuery, IEnumerable<EnumDTO>>
{
    public Task<IEnumerable<EnumDTO>> Handle(
        GetJobTypesQuery request,
        CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<JobType>()
            .Select(p => new EnumDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IEnumerable<EnumDTO>>(result);
    }
}