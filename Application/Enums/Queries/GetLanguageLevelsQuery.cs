﻿using Application.Common.DTO;
using Domain.Enums;
using MediatR;

namespace Application.Enums.Queries;

public record GetLanguageLevelsQuery : IRequest<IList<EnumDTO>>;

public class GetLanguageLevelsQueryHandler : IRequestHandler<GetLanguageLevelsQuery, IList<EnumDTO>>
{
    public GetLanguageLevelsQueryHandler() { }

    public Task<IList<EnumDTO>> Handle(GetLanguageLevelsQuery request, CancellationToken cancellationToken)
    {
        var result = Enum.GetValues<LanguageLevel>()
            .Select(p => new EnumDTO { Value = (int)p, Name = p.ToString() })
            .ToList();

        return Task.FromResult<IList<EnumDTO>>(result);
    }
}