namespace API.DTO.Requests.Posts;

public record UpdateOwnPostRequest
{
    public Guid PostId { get; init; }
    public string Text { get; init; } = string.Empty;
}
