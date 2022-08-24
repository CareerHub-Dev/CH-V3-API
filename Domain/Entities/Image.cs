using Domain.Common;

namespace Domain.Entities;

public class Image : BaseAuditableEntity
{
    public string ContentType { get; set; } = string.Empty;
    public byte[] Content { get; set; } = Array.Empty<byte>();
}
