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
        var claimsIdentity = new ClaimsIdentity();
        var claimType = "AccountType";

        if (!principal.HasClaim(claim => claim.Type == claimType))
        {
            var user = await _userManager.GetUserAsync(principal);
            if (user != null)
            {
                var claim = new Claim(
                    type: claimType,
                    value: user.AccountType.ToString()
                );
                claimsIdentity.AddClaim(claim);
            }
        }

        principal.AddIdentity(claimsIdentity);
        return principal;
    }
}