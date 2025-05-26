using AcademySpacesAPI.ApplicationCore.DomainEntities;
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
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public ParentController(ICreateParentUseCase createParentUseCase, IHttpContextAccessor httpContextAccessor)
    {
        _createParentUseCase = createParentUseCase;
        _httpContextAccessor = httpContextAccessor;
    }

    [HasPermission("Parent:create")]
    [HttpPost("create-parent")]
    public async Task<IActionResult> CreateParent(CreateParentRequest request)
    {
        var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);
        
        await _createParentUseCase.CreateParentAsync(new ParentEntry
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email,
        }, schoolId);
        
        return Ok(new
        {
            Status = true,
            Message = "Parent created and invited.",
            Data = (object)null,
            Errors = (string[])null
        });
    }
}