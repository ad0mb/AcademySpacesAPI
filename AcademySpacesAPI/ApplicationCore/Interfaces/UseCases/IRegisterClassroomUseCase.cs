using AcademySpacesAPI.WebApi.DTOs;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;

public interface IRegisterClassroomUseCase
{
    Task CreateClassroomAsync(CreateClassroomRequest request);
}