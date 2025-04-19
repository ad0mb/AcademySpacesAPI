using System.Net.Http.Headers;
using System.Security.Claims;
using AcademySpacesAPI.Data.Auth;
using AcademySpacesAPI.Services.Firebase.Auth;
using FirebaseAdmin.Auth;

namespace AcademySpacesAPI.Middleware;

public class Middleware
{
    private readonly RequestDelegate _next;
    
    
    public Middleware(RequestDelegate next)
    {
        throw new NotSupportedException("Middleware is not supported. Use FirebaseAuthenticationHandler instead.");
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, FirebaseAuthService firebaseAuthService)
    {
        Console.WriteLine("Middleware is executing.");
        
        ClaimsPrincipal principal;
        
        var idToken = httpContext.Request.Cookies["access"];
        
        if (idToken == null)
        {
            httpContext.Response.StatusCode = 401;
            await httpContext.Response.WriteAsync("Unauthorized access token missing");
            return;
        }
        
        try
        {
            principal = await firebaseAuthService.ProcessIdTokenAsync(idToken);
        }
        catch (FirebaseAuthException ex)
        {
            Console.WriteLine(ex);
            httpContext.Response.StatusCode = 401;
            await httpContext.Response.WriteAsync(ex.Message);
            return;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex);
            httpContext.Response.StatusCode = 403;
            await httpContext.Response.WriteAsync(ex.Message);
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            httpContext.Response.StatusCode = 500;
            await httpContext.Response.WriteAsync("Unplanned error: " + ex.Message);
            return;
        }
        
        httpContext.User = principal;
        
        await _next(httpContext);
        Console.WriteLine("Middleware completed successfully.");
    }
    
    
}