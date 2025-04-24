using System.Security.Claims;
using System.Text.Encodings.Web;
using AcademySpacesAPI.Infrastructure.Firebase;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace AcademySpacesAPI.WebApi.Authentication;

public class DefaultAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly FirebaseAuthService _firebaseAuthService;

    public DefaultAuthenticationHandler(FirebaseAuthService firebaseAuthService,
        IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder,
        ISystemClock clock) : base (options, logger, encoder, clock) //Base is the way to access the implemented classes constructor
    {
        _firebaseAuthService = firebaseAuthService;
    }
    
    //Check AuthenticationHandler.cs for more information on overriding other elements of the AuthenticationHandler interface.
    
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        ClaimsPrincipal principal;
        
        var idToken = Context.Request.Cookies["access"];
        
        if (idToken == null)
        {
            return AuthenticateResult.Fail("Id token is missing");
        }

        //TODO: Look into checking token expiration before processing it (most likely not to implement because cookie on frontend contains expiration date)
        //TODO: Make catches return to a future logger and the entire exception not just a message
        //Fix issue where AuthenticateResult.Fail does not actually return error message
        try
        {
            principal = await _firebaseAuthService.ProcessIdTokenAsync(idToken);
        }
        catch (FirebaseAuthException ex)
        {
            return AuthenticateResult.Fail(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return AuthenticateResult.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            //TODO: Handle AuthAccess exception
            return AuthenticateResult.Fail("Unplanned error occured: " + ex.Message);
        }

        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return AuthenticateResult.Success(ticket);

    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Context.Response.StatusCode = 401;
        await Context.Response.WriteAsync("Unauthenticated");
        await Task.CompletedTask;
    }

    protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        Context.Response.StatusCode = 403;
        await Context.Response.WriteAsync("Forbidden, Insufficient permissions to access resource");
        await Task.CompletedTask;
    }
}