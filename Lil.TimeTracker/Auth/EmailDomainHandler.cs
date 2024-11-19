using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Lil.TimeTracker.Auth;

public class EmailDomainHandler : AuthorizationHandler<EmailDomainRequirement>
{
    protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmailDomainRequirement requirement) {
        var emailClaim = context.User.FindFirst(c => c.Type == ClaimTypes.Email);

        if (emailClaim == null) {
            return;
        }

        if (emailClaim.Value.EndsWith(requirement.Domain, StringComparison.InvariantCultureIgnoreCase)) {
            context.Succeed(requirement);
        }
        
        return;
    }
}