using Microsoft.AspNetCore.Authorization;
using NationalParksApi.Models;
using System;
using System.Threading.Tasks;

public class SameOwnerHandler : AuthorizationHandler<OwnerOrAdminRequirement, NationalPark>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerOrAdminRequirement requirement, NationalPark resource)
    {
        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var currentUser = context.User.Identity?.Name;
        if (!string.IsNullOrEmpty(currentUser) &&
            resource.CreatedBy.Equals(currentUser, StringComparison.OrdinalIgnoreCase))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
