using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AcademySpacesAPI.Models;

public class RolePermissions
{
    [Required] [JsonIgnore] public int Id { get; set; }
    [Required] public int RoleId { get; set; }
    [Required] public required string PermissionName { get; set; }
    [Required] public bool Create { get; set; }
    [Required] public bool Delete { get; set; }
    [Required] public bool Update { get; set; }
}