using System.Security.Claims;
using AcademySpacesAPI.ApplicationCore.DomainEntities;
using AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;
using AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;
using AcademySpacesAPI.Exceptions;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using AcademySpacesAPI.WebApi.DTOs.Responses;

namespace AcademySpacesAPI.ApplicationCore.UseCases;

public class PostUserPreferencesUseCase : IPostUserPreferencesUseCase
{
    
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebPreferencesRepository _webPreferencesRepository;
    
    public PostUserPreferencesUseCase(IHttpContextAccessor httpContextAccessor, IWebPreferencesRepository webPreferencesRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _webPreferencesRepository = webPreferencesRepository;
    }

    public async Task<GetUserPreferencesResponse> PostUserPreferencesAsync(PostUserPreferencesRequest request)
    {
        var identityId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        WebPreferencesEntry userPreferences;
        
        try
        {
            userPreferences = await _webPreferencesRepository.UpdateWebPreferencesAsync(new WebPreferencesEntry
            {
                IdentityId = identityId,
                PageBrightness = request.PageBrightness,
                Locale = request.Locale
            });
        } catch (NotFoundException ex)
        {
            userPreferences = await _webPreferencesRepository.AddWebPreferencesAsync(new WebPreferencesEntry
            {
                IdentityId = identityId,
                PageBrightness = request.PageBrightness,
                Locale = request.Locale
            });
        }
        
        return new GetUserPreferencesResponse
        {
            PageBrightness = userPreferences.PageBrightness,
            Locale = userPreferences.Locale,
        };
    }
}