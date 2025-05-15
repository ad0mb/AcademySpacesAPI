using AcademySpacesAPI.WebApi.DTOs.Requests;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;

public interface IRegisterFacultyUseCase
{
    Task CreateFacultyAsync(InviteFacultyRequest request);
}