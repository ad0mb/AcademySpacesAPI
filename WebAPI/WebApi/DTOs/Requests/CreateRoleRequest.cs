using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class CreateRoleRequest
{
    [Required] public string RoleName { get; set; }
    public string? RoleDescription { get; set; }
    [Required] public RoleCategories Permissions { get; set; }
}

public class RoleCategories
{
    [Required] public CreatePermissionRequest Roles { get; set; }
    [Required] public CreatePermissionRequest Classrooms { get; set; }
}