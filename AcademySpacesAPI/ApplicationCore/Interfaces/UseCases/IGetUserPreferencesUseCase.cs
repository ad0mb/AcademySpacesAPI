using AcademySpacesAPI.WebApi.DTOs.Requests;
using AcademySpacesAPI.WebApi.DTOs.Responses;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;

public interface IGetUserPreferencesUseCase
{
    Task<GetUserPreferencesResponse> GetUserPreferencesAsync();
}