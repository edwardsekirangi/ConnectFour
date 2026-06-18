using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace ConnectFour.Services;

public class DefaultAuthenticationStateProvider : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "default-user-id"),
            new Claim(ClaimTypes.Name, "DefaultUser")
        };

        var identity = new ClaimsIdentity(claims, "DefaultAuth");
        var user = new ClaimsPrincipal(identity);
        return Task.FromResult(new AuthenticationState(user));
    }
}
