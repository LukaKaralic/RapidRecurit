using Microsoft.AspNetCore.Authorization;
using RapidRecruit.Models;
using System.Security.Claims;

namespace RapidRecruit.Authorization
{
    public class OwnerHandler : AuthorizationHandler<OwnerRequirement, JobPosting>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OwnerRequirement requirement,
            JobPosting jobPosting)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == jobPosting.UserId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
