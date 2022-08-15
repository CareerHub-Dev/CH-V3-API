using Domain.Common;

namespace Domain.Events.Image;

public class ImageDeletedEvent : BaseEvent
{
    public ImageDeletedEvent(Entities.Image image)
    {
        Image = image;
    }

    public Entities.Image Image { get; }
}
