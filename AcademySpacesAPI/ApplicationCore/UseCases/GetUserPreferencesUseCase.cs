using System.Security.Claims;
using AcademySpacesAPI.ApplicationCore.DomainEntities;
using AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;
using AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;
using AcademySpacesAPI.WebApi.DTOs.Responses;

namespace AcademySpacesAPI.ApplicationCore.UseCases;

public class GetUserPreferencesUseCase : IGetUserPreferencesUseCase
{
    
    private readonly IWebPreferencesRepository _webPreferencesRepository;
    
    public GetUserPreferencesUseCase(IWebPreferencesRepository webPreferencesRepository)
    {
        _webPreferencesRepository = webPreferencesRepository;
    }
    
    public async Task<WebPreferencesEntry> GetUserPreferencesAsync(string identityId)
    {
        var preferences = await _webPreferencesRepository.GetWebPreferencesAsync(identityId);
        
        if (preferences == null)
        {
            return new WebPreferencesEntry();
        }

        return preferences;
    }
}