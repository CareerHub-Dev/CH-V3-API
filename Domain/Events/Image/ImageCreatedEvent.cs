using Domain.Common;

namespace Domain.Events.Image;

public class ImageCreatedEvent : BaseEvent
{
    public ImageCreatedEvent(Entities.Image image)
    {
        Image = image;
    }

    public Entities.Image Image { get; }
}
