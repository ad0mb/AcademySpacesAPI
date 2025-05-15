using AcademySpacesAPI.WebApi.Attributes;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.WebApi.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
[Route("api/user/faculty")]
public class ClassroomController : ControllerBase
{
    public ClassroomController()
    {
        
    }
    
    [HasPermission("Classroom:create")]
    [HttpGet("create-classroom")]
    public IActionResult CreateClassroom(CreateClassroomRequest request)
    {
        throw new NotImplementedException();
        
        return Ok(new
        {
            Status = true,
            Message = "Classroom created successfully.",
            Data = new {},
            Errors = (string[])null
        });
    }
}