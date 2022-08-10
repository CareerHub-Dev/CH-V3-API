namespace Domain.Entities;

public class Student : Account
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Guid? Photo { get; set; }
    public string Phone { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
}
