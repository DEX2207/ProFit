using System.Security.Claims;
using ProFit.Domain.Models;

namespace ProFit.Domain.Helpers;

public class AuthenticateUserHelper
{
    public static ClaimsIdentity Authenticate(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(ClaimTypes.Name,user.Login),
            new Claim(ClaimsIdentity.DefaultNameClaimType,user.Role.ToString()),
        };
        return new ClaimsIdentity(claims, "ApplicationCooke", ClaimTypes.Email, ClaimsIdentity.DefaultRoleClaimType);
    }
}