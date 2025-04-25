using AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;
using AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;
using AcademySpacesAPI.WebApi.DTOs;

namespace AcademySpacesAPI.ApplicationCore.UseCases;

public class RegisterFacultyUseCase : IRegisterFacultyUseCase
{
    private readonly IEmailService _emailService;
    public RegisterFacultyUseCase(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task CreateFacultyAsync(InviteFacultyRequest request)
    {
        throw new NotImplementedException();
    }
}