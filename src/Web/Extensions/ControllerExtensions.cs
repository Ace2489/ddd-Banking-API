using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Web.Extensions;

public static class ControllerExtensions
{
    public static Guid GetLoggedInUser(this ControllerBase controller)
    {
        Claim? userClaim = controller.User.FindFirst(ClaimTypes.NameIdentifier) ?? throw new Exception("No logged-in user could be found");

        Guid userId = Guid.Parse(userClaim.Value);

        return userId;
    }
}
