using System.Text.Json;
using AcademySpacesAPI.WebApi.Attributes;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.UseCases;
using Core.Exceptions;
using Infrastructure.Infrastructure.Persistence.Entities;
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
    private readonly ICreateRoleUseCase _createRoleUseCase;

    public RolesController(IHttpContextAccessor accessor, IGetRolesUseCase getRolesUseCase, ICreateRoleUseCase createRoleUseCase)
    {
        _httpContextAccessor = accessor;
        _getRolesUseCase = getRolesUseCase;
        _createRoleUseCase = createRoleUseCase;
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

    [HasPermission("Roles:create")]
    [HttpPost("create-role")]
    public async Task<IActionResult> CreateRole(CreateRoleRequest request)
    {
        var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);
        
        var permissions = new List<RolePermissionEntry>();

        var jsonString = JsonSerializer.Serialize(request);
        var json = JsonSerializer.Deserialize<JsonElement>(jsonString);

        foreach (var permission in json.GetProperty("Permissions").EnumerateObject())
        {
            if (permission.Value.GetProperty("View").GetBoolean())
            {
                permissions.Add(new RolePermissionEntry
                {
                    PermissionName = permission.Name.ToLower(),
                    Create = permission.Value.GetProperty("Create").GetBoolean(),
                    Delete = permission.Value.GetProperty("Delete").GetBoolean(),
                    Update = permission.Value.GetProperty("Update").GetBoolean(),
                });
            }
        }

        try
        {
            await _createRoleUseCase.CreateRoleAsync(schoolId, request.RoleName, request.RoleDescription, permissions);
            
            return Ok( new
                {
                    Status = true,
                    Message = "Created role successfully.",
                    Data = (object[])null,
                    Errors = (string[])null
                }
            );
        }
        catch (DbException ex)
        {
            return StatusCode(500, new
            {
                Status = false,
                Message = "Internal server error.",
                Data = (object[])null,
                Errors = new[] { ex.Message }
            });
        }
        catch (DuplicateNameException ex)
        {
            return StatusCode(409, new
            {
                Status = false,
                Message = ex.Message,
                Data = (object[])null,
                Errors = new[] { ex.Message }
            });
        }
    }
}