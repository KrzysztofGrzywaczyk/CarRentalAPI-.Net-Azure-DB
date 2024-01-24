using System.Security.Claims;

namespace CarRentalAPI.Services;

public interface IUserContextService
{
    public ClaimsPrincipal User { get; }

    public int? GetUserId { get; }
}
