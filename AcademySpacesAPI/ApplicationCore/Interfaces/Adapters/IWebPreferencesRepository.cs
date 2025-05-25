using AcademySpacesAPI.ApplicationCore.DomainEntities;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;

public interface IWebPreferencesRepository
{
    Task<WebPreferencesEntry?> GetWebPreferencesAsync(string identityId);
    Task<WebPreferencesEntry> AddWebPreferencesAsync(WebPreferencesEntry webPreferencesEntry);
    Task<WebPreferencesEntry> UpdateWebPreferencesAsync(WebPreferencesEntry webPreferencesEntry);
}