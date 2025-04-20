using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AcademySpacesAPI.Models.DatabaseModels;

public class Schools
{
    public int SchoolId { get; set; }
    public int OrganizationId { get; set; }
    [Required] public required string Name { get; set; }
    [Required] public required string CountryOfOrigin { get; set; }
    [JsonIgnore] public DateTime? CreatedAt { get; set; }
    [JsonIgnore] public DateTime? UpdatedAt { get; set; }
}