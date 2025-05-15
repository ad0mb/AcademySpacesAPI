using System.Security.Claims;
using AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;
using AcademySpacesAPI.Infrastructure.Auth;
using AcademySpacesAPI.WebApi.Attributes;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.WebApi.Controllers.Users;

[ApiController]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
[Route("api/user/faculty")]
public class FacultyController : ControllerBase
{

    private readonly AuthService _authService;
    private readonly IRegisterFacultyUseCase _registerFacultyUseCase;

    public FacultyController(IRegisterFacultyUseCase registerFacultyUseCase)
    {
        _registerFacultyUseCase = registerFacultyUseCase;
    }

    [HasPermission("Faculty:create")]
    [HttpPost("invite-faculty")]
    public async Task<IActionResult> InviteFaculty(InviteFacultyRequest request)
    {
        throw new NotImplementedException();
        
        return Ok(new
        {
            Status = true,
            Message = "Faculty created and invited.",
            Data = (object)null,
            Errors = (string[])null
        });
    }



}
