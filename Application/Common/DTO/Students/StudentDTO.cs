namespace Application.Common.DTO.Students;

public class StudentDTO : StudentDetailedDTO
{
    public DateTime? Verified { get; set; }
    public DateTime? PasswordReset { get; set; }
}
