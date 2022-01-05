using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ForumDbContext.Model.DTO;
using System.Security.Claims;

namespace ForumWebAPI.BL.Auth {
    public class QuestionAuthorizationHandler : AuthorizationHandler<SameAuthorRequirement, QuestionDbDTO> {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SameAuthorRequirement requirement, QuestionDbDTO resource) {
            if (requirement.RolesThatCanModify.Any(role => context.User.HasClaim(ClaimTypes.Role, role))) {
                context.Succeed(requirement);
            }
            if (context.User.HasClaim(ClaimTypes.NameIdentifier, resource.AuthorId)) {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
