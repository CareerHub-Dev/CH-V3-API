using Application.Common.Interfaces;
using API.Authorize;

namespace API.Services;

public class CurrentAccountService : ICurrentAccountService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentAccountService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? AccountId => (_httpContextAccessor.HttpContext?.Items["Account"] as AccountInfo)?.Id;
}
