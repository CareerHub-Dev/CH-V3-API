using Microsoft.AspNetCore.Mvc;

namespace API.Filters;

public class BanProblemDetails : ProblemDetails
{
    public string Reasone { get; set; } = string.Empty;
    public DateTime? Expires { get; set; }
}
