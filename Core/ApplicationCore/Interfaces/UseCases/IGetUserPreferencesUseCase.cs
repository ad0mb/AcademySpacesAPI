using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetUserPreferencesUseCase
{
    Task<WebPreferencesEntry> GetUserPreferencesAsync(string identityId);
}