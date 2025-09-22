using System.Reflection;
using System.Text.Json;
using AcademySpacesAPI.WebApi.Attributes;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.UseCases;
using Core.ApplicationCore.UseCases;
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
    private readonly ICreateRoleUseCase _createRoleUseCase;
    private readonly IGetUserRolesPermissionsUseCase _getUserRolesPermissionsUseCase;
    private readonly IUpdateRoleUseCase _updateRoleUseCase;

    public RolesController(IHttpContextAccessor accessor, IGetRolesUseCase getRolesUseCase, ICreateRoleUseCase createRoleUseCase, IGetUserRolesPermissionsUseCase getUserRolesPermissionsUseCase, IUpdateRoleUseCase updateRoleUseCase)
    {
        _httpContextAccessor = accessor;
        _getRolesUseCase = getRolesUseCase;
        _createRoleUseCase = createRoleUseCase;
        _getUserRolesPermissionsUseCase = getUserRolesPermissionsUseCase;
        _updateRoleUseCase = updateRoleUseCase;
    }

    [HasPermission("Roles:view")]
    [HttpGet("get-roles")]
    public async Task<IActionResult> GetRoles()
    {
        var result = new List<CreateRoleRequest>();
        
        var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

        try
        {
            var roles = await _getRolesUseCase.GetRolesAsync(schoolId);

            foreach (var role in roles)
            {
                var permissions = await _getUserRolesPermissionsUseCase.GetUserPermissionsByRoleIdAsync(role.RoleId);

                var roleCategories = new RoleCategories();
                var createRoleRequest = new CreateRoleRequest
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                    RoleDescription = role.RoleDescription,
                    Permissions = roleCategories
                };

                if (permissions != null && permissions.Count > 0)
                {
                    var type = roleCategories.GetType();

                    foreach (var permission in permissions)
                    {
                        var property = type.GetProperty(permission.PermissionName,
                            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        
                        if (property != null)
                        {
                            property.SetValue(createRoleRequest.Permissions, new CreatePermissionRequest
                            {
                                Id = permission.Id,
                                View = true,
                                Create = permission.Create,
                                Delete = permission.Delete,
                                Update = permission.Update
                            });
                        }
                    }
                }
                result.Add(createRoleRequest);
            }
            
            return Ok( new
                {
                    Status = true,
                    Message = "Retrieved roles successfully.",
                    Data = result,
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

    [HasPermission("Roles:update")]
    [HttpPatch("update-role")]
    public async Task<IActionResult> UpdateRole(CreateRoleRequest request)
    {
        var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);
        
        var permissions = new List<RolePermissionEntry>();
        var permissionsToDelete = new List<int>();

        var jsonString = JsonSerializer.Serialize(request);
        var json = JsonSerializer.Deserialize<JsonElement>(jsonString);

        foreach (var permission in json.GetProperty("Permissions").EnumerateObject())
        {
            if (permission.Value.GetProperty("View").GetBoolean())
            {
                permissions.Add(new RolePermissionEntry
                {
                    Id = permission.Value.GetProperty("Id").GetInt32(),
                    RoleId = request.RoleId,
                    PermissionName = permission.Name.ToLower(),
                    Create = permission.Value.GetProperty("Create").GetBoolean(),
                    Delete = permission.Value.GetProperty("Delete").GetBoolean(),
                    Update = permission.Value.GetProperty("Update").GetBoolean(),
                });
            }
            else
            {
                if (permission.Value.GetProperty("Id").GetInt32() > 0)
                {
                    permissionsToDelete.Add(permission.Value.GetProperty("Id").GetInt32());
                }
            }
        }

        try
        {
            await _updateRoleUseCase.UpdateRoleAsync(new RoleEntry
            {
                SchoolId = schoolId,
                RoleId = request.RoleId,
                RoleName = request.RoleName,
                RoleDescription = request.RoleDescription,
            }, permissions, permissionsToDelete);

            return Ok(new
                {
                    Status = true,
                    Message = "Updated role successfully.",
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
        catch (NotFoundException ex)
        {
            return StatusCode(404, new
            {
                Status = false,
                Message = ex.Message,
                Data = (object[])null,
                Errors = new[] { ex.Message }
            });
        }
    }
}