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

    public async Task<WebPreferencesEntry> PostUserPreferencesAsync(WebPreferencesEntry request)
    {
        var identityId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        WebPreferencesEntry userPreferences;
        
        request.IdentityId = identityId;
        
        try
        {
            userPreferences = await _webPreferencesRepository.UpdateWebPreferencesAsync(request);
        } catch (NotFoundException ex)
        {
            userPreferences = await _webPreferencesRepository.AddWebPreferencesAsync(request);
        }
        
        return userPreferences;
    }
}