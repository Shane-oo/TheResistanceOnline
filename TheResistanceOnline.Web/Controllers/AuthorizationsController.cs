using System.Security.Claims;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Client.AspNetCore;
using OpenIddict.Server.AspNetCore;
using TheResistanceOnline.Authentications.ExternalIdentities;
using TheResistanceOnline.Authentications.OpenIds;
using TheResistanceOnline.Core.Exchange.Responses;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;
using TheResistanceOnline.Data.Entities;
using OpenIddictErrors = OpenIddict.Abstractions.OpenIddictConstants.Errors;
using OpenIddictClaims = OpenIddict.Abstractions.OpenIddictConstants.Claims;
using OpenIddictDestinations = OpenIddict.Abstractions.OpenIddictConstants.Destinations;
using OpenIddictScopes = OpenIddict.Abstractions.OpenIddictConstants.Scopes;
using OpenIddictWebProviders = OpenIddict.Client.WebIntegration.OpenIddictClientWebIntegrationConstants.Providers;

namespace TheResistanceOnline.Web.Controllers;

public class AuthorizationsController: Controller
{
    #region Constants

    public const string COOKIE_NAME = "TheResistanceOnline.Web.Cookie";


    private const string AUTH_SCHEME = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;
    private const string COOKIE_AUTH_SCHEME = CookieAuthenticationDefaults.AuthenticationScheme;
    private const string OI_CLIENT_AUTH_SCHEME = OpenIddictClientAspNetCoreDefaults.AuthenticationScheme;

    #endregion

    #region Fields

    private readonly IMediator _mediator;

    #endregion

    #region Construction

    public AuthorizationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #endregion

    #region Private Methods

    private ForbidResult Forbid(string error, string description)
    {
        return Forbid(new AuthenticationProperties(new Dictionary<string, string>
                                                   {
                                                       [OpenIddictServerAspNetCoreConstants.Properties.Error] = error,
                                                       [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                                           description
                                                   }),
                      AUTH_SCHEME);
    }

    private async Task<IActionResult> ProcessAuthorizationCodeAuth(OpenIddictRequest request, CancellationToken cancellationToken)
    {
        // Retrieve the claims principal stored in the authorization code
        var principal = (await HttpContext.AuthenticateAsync(AUTH_SCHEME)).Principal;
        if (principal is null)
        {
            return Forbid(OpenIddictErrors.InvalidGrant, "Could Not Get Principal");
        }

        var userIdClaim = principal.GetClaim(OpenIddictClaims.Subject);
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Forbid(OpenIddictErrors.InvalidRequestObject, "Required Claim Not Found");
        }

        var result = await _mediator.Send(new AuthenticateUserWithCodeGrantCommand(new UserId(userId)), cancellationToken);

        return result.IsFailure ? Forbid(OpenIddictErrors.InvalidGrant, result.Error.Description) : SignInUser(request, result.Value);
    }

    private async Task<IActionResult> ProcessGoogleAuthorization(ClaimsIdentity claimsIdentity, CancellationToken cancellationToken)
    {
        //authenticate User With Google 
        var command = new AuthenticateUserWithGoogleCommand(claimsIdentity.GetClaim(CustomClaims.GoogleClaims.AUDIENCE),
                                                            new GoogleId(claimsIdentity.GetClaim(CustomClaims.GoogleClaims.SUBJECT)));

        var result = await _mediator.Send(command, cancellationToken);

        return SignInExternalAuthorization(result);
    }


    private async Task<IActionResult> ProcessMicrosoftAuthorization(ClaimsIdentity claimsIdentity, CancellationToken cancellationToken)
    {
        //authenticate User With Microsoft 
        var command = new AuthenticateUserWithMicrosoftCommand(claimsIdentity.GetClaim(CustomClaims.AzureADClaims.AUDIENCE),
                                                               Guid.TryParse(claimsIdentity.GetClaim(CustomClaims.AzureADClaims.OBJECT_ID), out var objectId)
                                                                   ? new MicrosoftId(objectId)
                                                                   : null);
        var result = await _mediator.Send(command, cancellationToken);

        return SignInExternalAuthorization(result);
    }

    private async Task<IActionResult> ProcessRedditAuthorization(ClaimsIdentity claimsIdentity, CancellationToken cancellationToken)
    {
        // authenticate User with Reddit
        var command = new AuthenticateUserWithRedditCommand(new RedditId(claimsIdentity.GetClaim(CustomClaims.RedditClaims.ID)),
                                                            claimsIdentity.GetClaim(CustomClaims.RedditClaims.AUDIENCE));
        var result = await _mediator.Send(command, cancellationToken);

        return SignInExternalAuthorization(result);
    }

    private IActionResult SignInExternalAuthorization(Result<UserId> result)
    {
        if (result.IsFailure)
        {
            HttpContext.Response.Cookies.Delete(COOKIE_NAME);
            return Forbid(OpenIddictErrors.AccessDenied, result.Error.Description);
        }

        var identity = new ClaimsIdentity(AUTH_SCHEME);

        identity.AddClaim(OpenIddictClaims.Subject, result
                                                    .Value
                                                    .Value.ToString());

        var principal = new ClaimsPrincipal(identity);

        // Delete cookie as no longer needed, forces user to sign in with microsoft again
        // regardless of cookie expiration time
        HttpContext.Response.Cookies.Delete(COOKIE_NAME);
        // Signing in with the OpenIddict authentication scheme trigger OpenIddict to issue a code (which can be exchanged for an access token)
        return SignIn(principal, AUTH_SCHEME);
    }

    private IActionResult SignInUser(OpenIddictRequest request, UserAuthenticationPayload payload)
    {
        var identity = new ClaimsIdentity(AUTH_SCHEME, OpenIddictClaims.Name, OpenIddictClaims.Role);

        identity.AddClaim(OpenIddictClaims.ClientId, request.ClientId!);
        identity.AddClaim(OpenIddictClaims.Subject, payload.UserId.Value.ToString());
        identity.AddClaim(OpenIddictClaims.Name, payload.UserName);
        identity.AddClaim(OpenIddictClaims.Role, payload.Role);

        // Allow all claims to be added in the access tokens.
        identity.SetDestinations(_ => new[] { OpenIddictDestinations.AccessToken, OpenIddictDestinations.IdentityToken });

        var principal = new ClaimsPrincipal(identity);
        principal.SetScopes(OpenIddictScopes.OfflineAccess, OpenIddictScopes.OpenId, OpenIddictScopes.Roles, OpenIddictScopes.Profile);

        // Application claims - that are potentially very sensitive - are only returned by OpenIddict 3.0 if all the following conditions are met:
        // The claims are present in the access token. This means you have to assign them the "access_token" destination before calling SignIn.
        // The application sending the introspection request was explicitly listed as a resource when calling SignIn
        // (i.e you called principal.SetResources("client_id of the API doing introspection")).
        // The API doing introspection was registered as a confidential client (i.e is forced to send a valid client_secret to be able to introspect a token).
        principal.SetResources("TheResistanceOnline.Server");

        return SignIn(principal, AUTH_SCHEME);
    }

    #endregion

    #region Public Methods

    [HttpGet("~/authorize")]
    [HttpPost("~/authorize")]
    public async Task<IActionResult> Authorize(CancellationToken cancellationToken)
    {
        var request = HttpContext.GetOpenIddictServerRequest();

        if (request is null)
        {
            return Forbid(OpenIddictErrors.InvalidRequestObject,
                          "The Authorize Request Could Not Be Retrieved.");
        }

        // Resolve the claims stored in the cookie created after the Providers authentication dance.
        // If the principal cannot be found, trigger a new challenge to redirect the user to the specified provider.
        var result = await HttpContext.AuthenticateAsync(COOKIE_AUTH_SCHEME);

        var principal = result.Principal;

        if (principal == null)
        {
            var provider = request.GetParameter("provider")?.Value?.ToString();
            if (string.IsNullOrEmpty(provider))
            {
                return Forbid(OpenIddictErrors.AccessDenied, "Missing Request Provider.");
            }

            var properties = new AuthenticationProperties(new Dictionary<string, string>
                                                          {
                                                              [OpenIddictClientAspNetCoreConstants.Properties.ProviderName] =
                                                                  provider
                                                          })
                             {
                                 RedirectUri = HttpContext.Request.GetEncodedUrl()
                             };
            return Challenge(properties, OI_CLIENT_AUTH_SCHEME);
        }

        return principal.Identity?.AuthenticationType switch
        {
            OpenIddictWebProviders.Microsoft => await ProcessMicrosoftAuthorization(principal.Identity as ClaimsIdentity, cancellationToken),
            OpenIddictWebProviders.Google => await ProcessGoogleAuthorization(principal.Identity as ClaimsIdentity, cancellationToken),
            OpenIddictWebProviders.Reddit => await ProcessRedditAuthorization(principal.Identity as ClaimsIdentity, cancellationToken),
            _ => Forbid(OpenIddictErrors.AccessDenied, "Provider Not Supported")
        };
    }

    [HttpPost("~/token")]
    [Produces("application/json")]
    public async Task<IActionResult> Exchange(CancellationToken cancellationToken)
    {
        var request = HttpContext.GetOpenIddictServerRequest();

        if (request is null)
        {
            return Forbid(OpenIddictErrors.InvalidRequestObject,
                          "The Token Request Could Not Be Retrieved");
        }

        if (request.IsAuthorizationCodeGrantType())
        {
            return await ProcessAuthorizationCodeAuth(request, cancellationToken);
        }

        if (request.IsRefreshTokenGrantType())
        {
            // Retrieve the claims principal stored in the refresh token
            var principal = (await HttpContext.AuthenticateAsync(AUTH_SCHEME)).Principal;
            if (principal is null)
            {
                return Forbid(OpenIddictErrors.InvalidGrant, "Count Not Get Principal");
            }

            //todo
            //await _mediator.Send(new UpdateUserLoginOn...)

            return SignIn(principal, AUTH_SCHEME);
        }

        return Forbid(OpenIddictErrors.UnsupportedGrantType, "Grant Type Not Accepted");
    }

    [HttpGet("~/callback/login/google")]
    [HttpPost("~/callback/login/google")]
    public async Task<IActionResult> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync(OI_CLIENT_AUTH_SCHEME);

        // copy the claims you want to preserve to your local authentication cookie
        var identity = new ClaimsIdentity(OpenIddictWebProviders.Google);
        identity.AddClaim(new Claim(CustomClaims.GoogleClaims.AUDIENCE, result.Principal?.FindFirstValue(CustomClaims.GoogleClaims.AUDIENCE) ?? string.Empty));
        identity.AddClaim(new Claim(CustomClaims.GoogleClaims.SUBJECT, result.Principal?.FindFirstValue(CustomClaims.GoogleClaims.SUBJECT) ?? string.Empty));

        return SignInExternalProviderCallback(new ClaimsPrincipal(identity), result.Properties?.RedirectUri);
    }

    [HttpGet("~/callback/login/microsoft")]
    [HttpPost("~/callback/login/microsoft")]
    public async Task<IActionResult> MicrosoftCallback()
    {
        var result = await HttpContext.AuthenticateAsync(OI_CLIENT_AUTH_SCHEME);

        // copy the claims you want to preserve to your local authentication cookie
        var identity = new ClaimsIdentity(OpenIddictWebProviders.Microsoft);
        identity.AddClaim(new Claim(CustomClaims.AzureADClaims.AUDIENCE, result.Principal?.FindFirstValue(CustomClaims.AzureADClaims.AUDIENCE) ?? string.Empty));
        identity.AddClaim(new Claim(CustomClaims.AzureADClaims.OBJECT_ID, result.Principal?.FindFirstValue(CustomClaims.AzureADClaims.OBJECT_ID) ?? string.Empty));

        return SignInExternalProviderCallback(new ClaimsPrincipal(identity), result.Properties?.RedirectUri);
    }

    [HttpGet("~/callback/login/reddit")]
    [HttpPost("~/callback/login/reddit")]
    public async Task<IActionResult> RedditCallback()
    {
        var result = await HttpContext.AuthenticateAsync(OI_CLIENT_AUTH_SCHEME);

        var identity = new ClaimsIdentity(OpenIddictWebProviders.Reddit);
        identity.AddClaim(new Claim(CustomClaims.RedditClaims.ID, result.Principal?.FindFirstValue(CustomClaims.RedditClaims.ID) ?? string.Empty));
        identity.AddClaim(new Claim(CustomClaims.RedditClaims.AUDIENCE, result.Principal?.FindFirstValue(CustomClaims.RedditClaims.AUDIENCE) ?? string.Empty));


        return SignInExternalProviderCallback(new ClaimsPrincipal(identity), result.Properties?.RedirectUri);
    }

    public SignInResult SignInExternalProviderCallback(ClaimsPrincipal claimsPrincipal, [CanBeNull] string redirectUri)
    {
        var properties = new AuthenticationProperties
                         {
                             RedirectUri = redirectUri
                         };
        return SignIn(claimsPrincipal, properties, COOKIE_AUTH_SCHEME);
    }

    #endregion
}

public static class CustomClaims
{
    public static class AzureADClaims
    {
        #region Constants

        public const string AUDIENCE = "aud";
        public const string NAME = "name";
        public const string OBJECT_ID = "oid";

        #endregion
    }

    public static class GoogleClaims
    {
        #region Constants

        public const string AUDIENCE = "aud";
        public const string SUBJECT = "sub";

        #endregion
    }

    public static class RedditClaims
    {
        #region Constants

        public const string AUDIENCE = "oauth_client_id";

        public const string ID = "id";

        #endregion
    }
}
