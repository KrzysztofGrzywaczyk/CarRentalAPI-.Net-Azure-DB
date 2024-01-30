using NLog.LayoutRenderers;
using System.Security.Claims;

namespace CarRentalAPI.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserContextService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public ClaimsPrincipal User => _contextAccessor.HttpContext?.User!;

    public int? GetUserId => User is null ? null : (int?)int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
}
