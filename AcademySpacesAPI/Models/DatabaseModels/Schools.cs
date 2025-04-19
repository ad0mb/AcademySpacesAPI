using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademySpacesAPI.Models.DatabaseModels;

public class Schools
{
    [Required] public int SchoolId { get; set; }
    public int? OrganizationId { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string CountryOfOrigin { get; set; }
}