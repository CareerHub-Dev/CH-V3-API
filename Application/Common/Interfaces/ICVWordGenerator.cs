using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface ICVWordGenerator
    {
        MemoryStream GenerateDocument(CV cv);
    }
}
