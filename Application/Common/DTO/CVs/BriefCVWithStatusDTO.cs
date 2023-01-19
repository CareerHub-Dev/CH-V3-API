using Domain.Enums;

namespace Application.Common.DTO.CVs;

public class BriefCVWithStatusDTO : BriefCVDTO
{
    public Review Status { get; set; }
}
