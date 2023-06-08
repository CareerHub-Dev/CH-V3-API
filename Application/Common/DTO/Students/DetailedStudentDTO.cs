using Application.Common.DTO.StudentGroups;
using Domain.Entities;

namespace Application.Common.DTO.Students;

public class DetailedStudentDTO : ShortStudentDTO
{
    public required string? Phone { get; set; }
    public required DateTime? BirthDate { get; set; }
    public static DetailedStudentDTO FromEntity(Student entity)
    {
        return new()
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Photo = entity.Photo,
            Email = entity.Email,
            Phone = entity.Phone,
            BirthDate = entity.BirthDate,
            StudentGroup = new BriefStudentGroupDTO { Id = entity.StudentGroup!.Id, Name = entity.StudentGroup.Name },
        };
    }
}
