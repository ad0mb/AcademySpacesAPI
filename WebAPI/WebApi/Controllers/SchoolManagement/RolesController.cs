using AcademySpacesAPI.WebApi.Attributes;
using Core.ApplicationCore.Interfaces.UseCases;
using Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.WebApi.Controllers;

[ApiController]
[Route("api/school/administration/roles")]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
public class RolesController : ControllerBase
{
    
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGetRolesUseCase _getRolesUseCase;

    public RolesController(IHttpContextAccessor accessor, IGetRolesUseCase getRolesUseCase)
    {
        _httpContextAccessor = accessor;
        _getRolesUseCase = getRolesUseCase;
    }

    [HasPermission("Roles:view")]
    [HttpGet("get-roles")]
    public async Task<IActionResult> GetRoles()
    {
        var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

        try
        {
            var roles = await _getRolesUseCase.GetRolesAsync(schoolId);
            
            return Ok( new
                {
                    Status = true,
                    Message = "Retrieved roles successfully.",
                    Data = roles,
                    Errors = (string[])null
                }
            );
        } catch (DbException ex)
        {
            return StatusCode(500, new
            {
                Status = false,
                Message = "Internal server error.",
                Data = (object[])null,
                Errors = new[] { ex.Message }
            });
        }
    }
}