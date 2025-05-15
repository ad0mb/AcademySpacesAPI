using AcademySpacesAPI.WebApi.DTOs.Requests;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;

public interface ICreateClassroomUseCase
{
    Task CreateClassroomAsync(CreateClassroomRequest request);
}