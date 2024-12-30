using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using RapidRecruit.Models;
using System.Security.Claims;

public class AccountTypeClaimsTransformation : IClaimsTransformation
{
    private readonly UserManager<UserAccount> _userManager;

    public AccountTypeClaimsTransformation(UserManager<UserAccount> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var claimType = "AccountType";
        if (!principal.HasClaim(claim => claim.Type == claimType))
        {
            var user = await _userManager.GetUserAsync(principal);
            if (user != null)
            {
                var identity = principal.Identity as ClaimsIdentity;
                var claim = new Claim(
                    type: claimType,
                    value: user.AccountType.ToString()
                );
                identity.AddClaim(claim);  // Add to existing identity instead of creating new one
            }
        }
        return principal;
    }
}