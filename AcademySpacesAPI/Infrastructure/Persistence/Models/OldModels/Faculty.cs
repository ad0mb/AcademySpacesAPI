using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AcademySpacesAPI.Infrastructure.Persistence.Models.OldModels;

public class Faculty
{
    public int FacultyId { get; set; }
    [Required] public int SchoolId { get; set; }
    public string? IdentityId { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    [Required] public string Email { get; set; }
    [JsonIgnore] public DateTime? CreatedAt { get; set; }
    [JsonIgnore] public DateTime? UpdatedAt { get; set; }
}