using MVMedia.Api.Models;

using System.Security.Claims;

namespace MVMedia.Api.Identity;

public static class ClaimsPrincipalExtension
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
       return int.Parse(user.FindFirst("id").Value);

    }
    public static string GetUserName(this ClaimsPrincipal user)
    {
        return user.FindFirst("username").Value;
    }
}
