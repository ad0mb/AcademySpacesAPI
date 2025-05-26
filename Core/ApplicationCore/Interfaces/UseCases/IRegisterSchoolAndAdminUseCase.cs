
namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IRegisterSchoolAndAdminUseCase
{
    Task CreateSchoolAndAdminAsync(string schoolName, string schoolCountry, string firstName, string lastName, string signinEmail, string identityId);
}