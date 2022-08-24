namespace WebUI.Common.Models;

public class PaginationParameters
{
    private int _pageNumber = 1;
    public int PageNumber
    {
        get
        {
            return _pageNumber;
        }
        set
        {
            if (value <= 0)
            {
                _pageNumber = 1;
            }
            else
            {
                _pageNumber = value;
            }
        }
    }

    const int maxPageSize = 50;
    private int _pageSize = 10;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            if(value > maxPageSize)
            {
                _pageSize = maxPageSize;
            }
            else if (value <= 0)
            {
                _pageSize = 1;
            }
            else
            {
                _pageSize = value;
            }
        }
    }
}
