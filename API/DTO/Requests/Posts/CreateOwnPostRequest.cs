namespace API.DTO.Requests.Posts;

public record CreateOwnPostRequest
{
    public string Text { get; init; } = string.Empty;
    public List<IFormFile> Images { get; init; } = new List<IFormFile>();
}
