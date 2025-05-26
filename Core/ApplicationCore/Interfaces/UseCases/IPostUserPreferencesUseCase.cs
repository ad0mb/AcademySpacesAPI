using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IPostUserPreferencesUseCase
{
    Task<WebPreferencesEntry> PostUserPreferencesAsync(WebPreferencesEntry request, string identityId);
}
