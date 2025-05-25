using AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;
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
    private readonly ICreateParentUseCase _createParentUseCase;
    
    public ParentController(ICreateParentUseCase createParentUseCase)
    {
        _createParentUseCase = createParentUseCase;
    }

    [HasPermission("Parent:create")]
    [HttpPost("create-parent")]
    public async Task<IActionResult> CreateParent(CreateParentRequest request)
    {
        await _createParentUseCase.CreateParentAsync(request);
        
        return Ok(new
        {
            Status = true,
            Message = "Parent created and invited.",
            Data = (object)null,
            Errors = (string[])null
        });
    }
}