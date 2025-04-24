using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AcademySpacesAPI.Infrastructure.Persistence.Models.OldModels;

public class Schools
{
    public int SchoolId { get; set; }
    public int OrganizationId { get; set; }
    [Required] public required string Name { get; set; }
    [Required] public required string CountryOfOrigin { get; set; }
    [JsonIgnore] public DateTime? CreatedAt { get; set; }
    [JsonIgnore] public DateTime? UpdatedAt { get; set; }
}