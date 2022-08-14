using Domain.Common;

namespace Domain.Events.Image;

public class ImageUpdatedEvent : BaseEvent
{
    public ImageUpdatedEvent(Entities.Image image)
    {
        Image = image;
    }

    public Entities.Image Image { get; }
}
