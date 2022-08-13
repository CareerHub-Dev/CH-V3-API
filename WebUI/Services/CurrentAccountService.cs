using Application.Common.Interfaces;
using WebUI.Authorize;

namespace WebUI.Services;

public class CurrentAccountService : ICurrentAccountService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentAccountService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? AccountId => (_httpContextAccessor.HttpContext?.Items["Account"] as AccountInfo)?.Id;
}
