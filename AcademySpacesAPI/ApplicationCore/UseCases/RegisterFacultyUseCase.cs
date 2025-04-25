using AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;
using AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;
using AcademySpacesAPI.WebApi.DTOs;

namespace AcademySpacesAPI.ApplicationCore.UseCases;

public class RegisterFacultyUseCase : IRegisterFacultyUseCase
{
    private readonly IEmailService _emailService;
    private readonly IAuthService _authService;

    public RegisterFacultyUseCase(IEmailService emailService, IAuthService authService)
    {
        _emailService = emailService;
        _authService = authService;
    }

    public async Task CreateFacultyAsync(InviteFacultyRequest request)
    {
        throw new NotImplementedException();
    }
}