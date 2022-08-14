namespace Domain.Events.Image;

public class ImageDeletedEvent
{
    public ImageDeletedEvent(Entities.Image image)
    {
        Image = image;
    }

    public Entities.Image Image { get; }
}
