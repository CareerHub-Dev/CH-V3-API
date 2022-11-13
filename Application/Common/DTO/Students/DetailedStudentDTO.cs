namespace Application.Common.DTO.Students;

public class DetailedStudentDTO : ShortStudentDTO
{
    public string? Phone { get; set; }
    public DateTime? BirthDate { get; set; }
}
