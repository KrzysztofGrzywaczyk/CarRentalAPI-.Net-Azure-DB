using CarRentalAPI.Authorization;
using CarRentalAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CarRentalAPI.Authorizationl;

public class CarResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Car>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Car car)
    {
        if (requirement.Operation == ResourceOperation.Get ||
               requirement.Operation == ResourceOperation.Create)
        {
            context.Succeed(requirement);
        }

        var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            throw new ArgumentException("You must be registered user to edit or remove this car resource");
        }

        if (car.CreatedById == int.Parse(userId) ||
            car.ManagedById == int.Parse(userId) ||
                context.User.IsInRole("administrator"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
