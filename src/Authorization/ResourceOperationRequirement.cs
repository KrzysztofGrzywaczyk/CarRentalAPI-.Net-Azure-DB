using Microsoft.AspNetCore.Authorization;

namespace CarRentalAPI.Authorization;

public enum ResourceOperation
{
    Create,
    Get,
    Update,
    Delete,
}
public class ResourceOperationRequirement : IAuthorizationRequirement
{
    public ResourceOperationRequirement(ResourceOperation operation)
    {
        Operation = operation;
    }
    public ResourceOperation Operation { get; } 
}
