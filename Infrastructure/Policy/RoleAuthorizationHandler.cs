using System.Linq;
using System.Threading.Tasks;
using Domain.BaseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Policy
{
    public class RoleAuthorizationHandler : AuthorizationHandler<AuthorizationRequirements>
    {
        private readonly UserManager<BaseUser> _userManager;

        public RoleAuthorizationHandler(UserManager<BaseUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AuthorizationRequirements requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
                return;
            var userAsync = await _userManager.GetUserAsync(context.User);
            var userRoles = await _userManager.GetRolesAsync(userAsync);
            userRoles ??= new string[0];

            if (requirement.RoleNames.Intersect(userRoles).ToList().Count != 0)
                context.Succeed(requirement);
        }
    }
}