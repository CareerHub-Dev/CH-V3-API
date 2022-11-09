using Microsoft.AspNetCore.Mvc;

namespace API.ExceptionFilter.Models;

public class BanProblemDetails : ProblemDetails
{
    public string Reasone { get; set; } = string.Empty;
    public DateTime? Expires { get; set; }
}
