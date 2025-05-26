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
    
    private readonly IWebPreferencesRepository _webPreferencesRepository;
    
    public PostUserPreferencesUseCase(IWebPreferencesRepository webPreferencesRepository)
    {
        _webPreferencesRepository = webPreferencesRepository;
    }

    public async Task<WebPreferencesEntry> PostUserPreferencesAsync(WebPreferencesEntry request, string identityId)
    {
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