using AcademySpacesAPI.WebApi.DTOs;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;

public interface ICreateStudentUseCase
{
    Task CreateStudentAsync(CreateStudentRequest request);
}