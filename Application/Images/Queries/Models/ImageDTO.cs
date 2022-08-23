namespace Application.Images.Queries.Models;

public class ImageDTO
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public byte[] Content { get; set; } = Array.Empty<byte>();

    public string Extention => Path.GetExtension(FileName).Trim();
}
