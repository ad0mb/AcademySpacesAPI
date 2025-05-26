using AcademySpacesAPI.ApplicationCore.DomainEntities;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using AcademySpacesAPI.WebApi.DTOs.Responses;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;

public interface IGetUserPreferencesUseCase
{
    Task<WebPreferencesEntry> GetUserPreferencesAsync(string identityId);
}