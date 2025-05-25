using System.Security.Claims;
using AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;
using AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;
using AcademySpacesAPI.WebApi.DTOs.Responses;

namespace AcademySpacesAPI.ApplicationCore.UseCases;

public class GetUserPreferencesUseCase : IGetUserPreferencesUseCase
{
    
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebPreferencesRepository _webPreferencesRepository;
    
    public GetUserPreferencesUseCase(IHttpContextAccessor httpContextAccessor, IWebPreferencesRepository webPreferencesRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _webPreferencesRepository = webPreferencesRepository;
    }
    
    public async Task<GetUserPreferencesResponse> GetUserPreferencesAsync()
    {
        var identityId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        var preferences = await _webPreferencesRepository.GetWebPreferencesAsync(identityId);
        
        if (preferences == null)
        {
            return new GetUserPreferencesResponse();
        }

        return new GetUserPreferencesResponse
        {
            PageBrightness = preferences.PageBrightness,
            Locale = preferences.Locale,
        };
    }
}