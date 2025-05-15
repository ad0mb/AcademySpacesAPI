using AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;
using AcademySpacesAPI.WebApi.DTOs.Requests;

namespace AcademySpacesAPI.ApplicationCore.UseCases;

public class CreateStudentUseCase : ICreateStudentUseCase
{
    
    public CreateStudentUseCase()
    {
        
    }

    public Task CreateStudentAsync(CreateStudentRequest request)
    {
        throw new NotImplementedException();
    }
}