using Application.Common.DTO.Students;
using Domain.Enums;

namespace Application.Common.DTO.CVs;

public class BriefCVWithStatusAndStudentDTO : BriefCVWithStatusDTO
{
    public required DetailedStudentDTO Student { get; set; }
    public required Guid ReviewId { get; set; }
}
