using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class CreateRoleRequest
{
    public int RoleId { get; set; }
    [Required] public string RoleName { get; set; }
    public string? RoleDescription { get; set; }
    [Required] public RoleCategories Permissions { get; set; }
}

public class RoleCategories
{
    public RoleCategories()
    {
        Roles = new CreatePermissionRequest();
        Classrooms = new CreatePermissionRequest();
    }

    [Required] public CreatePermissionRequest Roles { get; set; }
    [Required] public CreatePermissionRequest Classrooms { get; set; }
}