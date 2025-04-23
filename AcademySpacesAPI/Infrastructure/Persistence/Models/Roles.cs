using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AcademySpacesAPI.Infrastructure.Persistence.Models;

public class Roles
{
    public int RoleId { get; set; }
    [Required] public required int SchoolId { get; set; }
    [Required] public required string RoleName { get; set; }
    [JsonIgnore] public DateTime? CreatedAt { get; set; }
    [JsonIgnore] public DateTime? UpdatedAt { get; set; }
}