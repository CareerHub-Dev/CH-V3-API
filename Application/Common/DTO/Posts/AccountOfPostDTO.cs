using Application.Common.Enums;

namespace Application.Common.DTO.Posts;

public class AccountOfPostDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public Role Role { get; set; }
}
