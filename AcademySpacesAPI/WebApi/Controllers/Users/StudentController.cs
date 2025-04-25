using AcademySpacesAPI.WebApi.Attributes;
using AcademySpacesAPI.WebApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.WebApi.Controllers.Users;

[ApiController]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
[Route("api/user/student")]
public class StudentController : ControllerBase
{
    public StudentController()
    {
        
    }
    
    [HasPermission("Student:create")]
    [HttpPost("create-student")]
    public async Task<IActionResult> CreateStudent(CreateStudentRequest request)
    {
        throw new NotImplementedException();
        
        return Ok(new
        {
            Status = true,
            Message = "Student created and invited.",
            Data = (object)null,
            Errors = (string[])null
        });
    }
}