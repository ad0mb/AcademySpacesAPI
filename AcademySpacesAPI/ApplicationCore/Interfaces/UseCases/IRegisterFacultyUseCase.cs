using AcademySpacesAPI.WebApi.DTOs;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;

public interface IRegisterFacultyUseCase
{
    Task CreateFacultyAsync(InviteFacultyRequest request);
}