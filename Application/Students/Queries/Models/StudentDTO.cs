namespace Application.Students.Queries.Models;

public class StudentDTO : StudentDetailedDTO
{
    public DateTime? Verified { get; set; }
    public DateTime? PasswordReset { get; set; }
}
