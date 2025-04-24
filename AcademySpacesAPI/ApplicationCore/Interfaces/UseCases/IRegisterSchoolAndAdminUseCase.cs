
using AcademySpacesAPI.WebApi.DTOs;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;

public interface IRegisterSchoolAndAdminUseCase
{
    Task CreateSchoolAndAdminAsync(RegisterSchoolRequest request);
}