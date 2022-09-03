namespace Domain.Entities;

public class Student : Account
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Guid? PhotoId { get; set; }
    public string? Phone { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }

    public Guid StudentGroupId { get; set; }
    public StudentGroup? StudentGroup { get; set; }

    public List<Company> CompanySubscriptions { get; set; } = new List<Company>();

    public List<StudentSubscription> StudentSubscriptions { get; set; } = new List<StudentSubscription>();
    public List<StudentSubscription> StudentsSubscribed { get; set; } = new List<StudentSubscription>();
}
