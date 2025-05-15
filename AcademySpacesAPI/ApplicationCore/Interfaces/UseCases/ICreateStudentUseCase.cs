using AcademySpacesAPI.WebApi.DTOs.Requests;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;

public interface ICreateStudentUseCase
{
    Task CreateStudentAsync(CreateStudentRequest request);
}