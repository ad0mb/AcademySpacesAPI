using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AcademySpacesAPI.Infrastructure.Persistence.Models;

public class RolePermissions
{
    [JsonIgnore] public int Id { get; set; }
    [Required] public required int RoleId { get; set; }
    [Required] public required string PermissionName { get; set; }
    [Required] public required bool Create { get; set; }
    [Required] public required bool Delete { get; set; }
    [Required] public required bool Update { get; set; }
    [JsonIgnore] public DateTime? CreatedAt { get; set; }
    [JsonIgnore] public DateTime? UpdatedAt { get; set; }
}