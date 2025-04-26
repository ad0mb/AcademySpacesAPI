using AcademySpacesAPI.WebApi.DTOs;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;

public interface ICreateClassroomUseCase
{
    Task CreateClassroomAsync(CreateClassroomRequest request);
}