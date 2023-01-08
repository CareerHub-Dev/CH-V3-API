using Domain.Common;

namespace Domain.Entities;

public class Post : BaseEntity
{
    public string? Text { get; set; }
    public List<string> Images { get; set; } = new List<string>();
    public DateTime CreatedDate { get; set; }

    public Guid AccountId { get; set; }
    public Account? Account { get; set; }

    public List<Student> StudentsLiked { get; set; } = new List<Student>();
}
