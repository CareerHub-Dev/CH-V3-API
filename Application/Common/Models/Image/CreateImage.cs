namespace Application.Common.Models.Image;

public class CreateImage
{
    public string ContentType { get; set; } = string.Empty;
    public byte[] Content { get; set; } = Array.Empty<byte>();
}
