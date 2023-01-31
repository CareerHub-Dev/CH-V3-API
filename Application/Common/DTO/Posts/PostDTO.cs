namespace Application.Common.DTO.Posts;

public class PostDTO
{
    public Guid Id { get; set; }
    public string? Text { get; set; }
    public List<string> Images { get; set; } = new List<string>();
    public DateTime CreatedDate { get; set; }
    public int Likes { get; set; }

    public AccountOfPostDTO Account { get; set; } = new AccountOfPostDTO();
}
