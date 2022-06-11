using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Utilities
{
    public static class Extensions
    {
        public static string GetUserId(this ControllerBase controllerBase)
        {
            return controllerBase.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}