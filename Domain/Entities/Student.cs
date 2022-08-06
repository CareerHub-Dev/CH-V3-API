using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Students")]
public class Student : Account
{
    [MaxLength(20)]
    public string FirstName { get; set; } = string.Empty;
    [MaxLength(20)]
    public string LastName { get; set; } = string.Empty;
    public Guid? Photo { get; set; }
    public string Phone { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
}
