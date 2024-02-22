using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace OpenIdDictDemo;

public class AuthorizationController : Controller
{
    [HttpPost("~/token")]
    public async ValueTask<IActionResult> Exchange()
    {
        //retrieve OIDC request from original request
        OpenIddictRequest request = HttpContext.GetOpenIddictServerRequest()
         ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (request.IsClientCredentialsGrantType())
        {
            string? clientId = request.ClientId;
            var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType);

            identity.SetClaim(OpenIddictConstants.Claims.Subject, clientId);
            identity.SetScopes(request.GetScopes());
            var principal = new ClaimsPrincipal(identity);
            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            SignInResult signInResult = SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            return await ValueTask.FromResult<IActionResult>(signInResult);
        }

        if (request.IsRefreshTokenGrantType())
        {
            AuthenticateResult authenticateResult =
                await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            ClaimsPrincipal? claimsPrincipal = authenticateResult.Principal;
            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        throw new NotImplementedException("The specified grant type is not implemented.");
    }
}