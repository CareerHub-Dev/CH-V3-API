namespace Application.Common.Models.Pagination;

public class PaginatedList<T> : List<T>
{
    public MetaData MetaData { get; set; }
    public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        MetaData = new MetaData
        {
            TotalCount = count,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };
        AddRange(items);
    }

    public PaginatedList(List<T> items, MetaData metaData)
    {
        MetaData = metaData;
        AddRange(items);
    }
}
