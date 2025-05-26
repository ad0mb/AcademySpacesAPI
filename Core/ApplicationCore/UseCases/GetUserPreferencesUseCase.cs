using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

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