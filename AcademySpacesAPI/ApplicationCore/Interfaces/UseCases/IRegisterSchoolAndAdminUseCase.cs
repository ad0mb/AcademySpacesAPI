
using AcademySpacesAPI.WebApi.DTOs.Requests;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;

public interface IRegisterSchoolAndAdminUseCase
{
    Task CreateSchoolAndAdminAsync(string schoolName, string schoolCountry, string firstName, string lastName, string signinEmail, string identityId);
}