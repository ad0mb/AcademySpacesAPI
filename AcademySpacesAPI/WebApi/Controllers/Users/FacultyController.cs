using System.Security.Claims;
using AcademySpacesAPI.Infrastructure.Auth;
using AcademySpacesAPI.WebApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.WebApi.Controllers.Users;

[ApiController]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
[Route("api/user/faculty")]
public class FacultyController : ControllerBase
{

    private readonly AuthService _authService;

    public FacultyController(AuthService authService)
    {
        _authService = authService;
    }

    [HasPermission("Faculty:create")]
    [HttpPost("invite-faculty")]
    public async Task<IActionResult> InviteFaculty(string request)
    {
        
        return Ok(new
        {
            Status = true,
            Message = "Faculty created and invited.",
            Data = (object)null,
            Errors = (string[])null
        });
    }



}
