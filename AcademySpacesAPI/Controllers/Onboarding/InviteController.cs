using AcademySpacesAPI.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.Controllers.Onboarding;

[ApiController]
[Route("api/onboarding/invite")]
public class InviteController : ControllerBase
{
    [Authorize(AuthenticationSchemes = "SchoolRegistrationBearer")]
    [HttpPost("register-school")]
    public IActionResult RegisterSchoolAndAdmin(RegisterRequest request)
    {
        return Ok(new 
        {
            Status = true,
            Message = "School and admin registered successfully.",
            Data = new { request },
            Errors = (string[])null
        });
    }
}