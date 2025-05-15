using AcademySpacesAPI.WebApi.Attributes;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.WebApi.Controllers.Users;

[ApiController]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
[Route("api/user/faculty")]
public class ParentController : ControllerBase
{
    public ParentController()
    {
        
    }

    [HasPermission("Parent:create")]
    [HttpPost("create-parent")]
    public async Task<IActionResult> CreateParent(CreateParentRequest request)
    {
        throw new NotImplementedException();
        
        return Ok(new
        {
            Status = true,
            Message = "Parent created and invited.",
            Data = (object)null,
            Errors = (string[])null
        });
    }
}