using System.Security.Claims;
using System.Text.Encodings.Web;
using FirebaseAdmin.Auth;
using Infrastructure.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace AcademySpacesAPI.WebApi.Authentication;

public class DefaultAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly AuthService _authService;
    private string errorType = "";

    public DefaultAuthenticationHandler(AuthService authService,
        IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder,
        ISystemClock clock) : base (options, logger, encoder, clock) //Base is the way to access the implemented classes constructor
    {
        _authService = authService;
    }
    
    //Check AuthenticationHandler.cs for more information on overriding other elements of the AuthenticationHandler interface.
    
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        ClaimsPrincipal principal;
        
        var idToken = Context.Request.Cookies["access"];
        var cycleIdOverride = Context.Request.Headers["X-Cycle-Override"].FirstOrDefault();
        
        if (idToken == null)
        {
            Console.WriteLine("not authenticated");
            errorType = "id_token_invalid";
            return AuthenticateResult.Fail("Id token is missing");
        }

        //TODO: Look into checking token expiration before processing it (most likely not to implement because cookie on frontend contains expiration date)
        //TODO: Make catches return to a future logger and the entire exception not just a message
        //Fix issue where AuthenticateResult.Fail does not actually return error message
        try
        {
            principal = await _authService.ProcessIdTokenAsync(idToken, cycleIdOverride);
        }
        catch (FirebaseAuthException ex)
        {
            errorType = "id_token_invalid";
            return AuthenticateResult.Fail(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            //TODO: LOG IT
            errorType = "claims_invalid";
            return AuthenticateResult.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            //TODO: LOG IT
            //TODO: Handle AuthAccess exception
            errorType = "unplanned_error";
            return AuthenticateResult.Fail("Unplanned error occured: " + ex.Message);
        }

        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return AuthenticateResult.Success(ticket);

    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Context.Response.StatusCode = 401;
        Context.Response.Headers.Add("Unauthorized-type", (StringValues) errorType);
        // Context.Response.Headers.Add("Access-Control-Expose-Headers", (StringValues) "Unauthorized-type");
        await Context.Response.WriteAsync("Unauthorized");
        await Task.CompletedTask;
    }

    protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        Context.Response.StatusCode = 403;
        await Context.Response.WriteAsync("Forbidden, Insufficient permissions to access resource");
        await Task.CompletedTask;
    }
}