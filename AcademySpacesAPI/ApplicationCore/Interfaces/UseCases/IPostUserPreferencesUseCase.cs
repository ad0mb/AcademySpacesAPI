using AcademySpacesAPI.ApplicationCore.DomainEntities;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using AcademySpacesAPI.WebApi.DTOs.Responses;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;

public interface IPostUserPreferencesUseCase
{
    Task<WebPreferencesEntry> PostUserPreferencesAsync(WebPreferencesEntry request);
}
