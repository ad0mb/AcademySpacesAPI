
namespace AcademySpacesAPI.Core.Interfaces.UseCases;

public interface IRegisterSchoolAndAdminUseCase
{
    Task CreateSchoolAndAdminAsync(string request);
}