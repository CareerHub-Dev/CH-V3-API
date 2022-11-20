namespace Application.Common.DTO.Bans;

public class BanDTO
{
    public Guid Id { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime Expires { get; set; }

    public Guid AccountId { get; set; }
}
