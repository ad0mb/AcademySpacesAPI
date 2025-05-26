using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;
using Core.Exceptions;

namespace Core.ApplicationCore.UseCases;

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