
using AcademySpacesAPI.WebApi.DTOs.Requests;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;

public interface IRegisterSchoolAndAdminUseCase
{
    Task CreateSchoolAndAdminAsync(RegisterSchoolRequest request);
}