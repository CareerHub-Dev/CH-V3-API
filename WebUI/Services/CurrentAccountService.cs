using Application.Common.Interfaces;

namespace WebUI.Services;

public class CurrentAccountService : ICurrentAccountService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentAccountService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? AccountId => _httpContextAccessor.HttpContext?.Items["AccountId"] as Guid?;
}
