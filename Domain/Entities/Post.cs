namespace Domain.Entities;

public class Post
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }

    public Guid AccoundId { get; set; }
    public Account? Account { get; set; }

    public List<Student> StudentsLiked { get; set; } = new List<Student>();
}
