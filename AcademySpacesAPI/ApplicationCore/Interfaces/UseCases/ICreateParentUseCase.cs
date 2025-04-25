using AcademySpacesAPI.WebApi.DTOs;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;

public interface ICreateParentUseCase
{
    Task CreateParentAsync(CreateParentRequest request);
}