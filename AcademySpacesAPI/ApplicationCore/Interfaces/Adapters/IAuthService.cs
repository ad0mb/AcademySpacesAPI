using System.Security.Claims;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;

public interface IAuthService
{
    Task<ClaimsPrincipal> ProcessIdTokenAsync(string idToken);
}