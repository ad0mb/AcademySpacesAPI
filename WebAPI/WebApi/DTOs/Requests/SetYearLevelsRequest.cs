using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class SetYearLevelsRequest
{
    public int Id { get; set; }
    [Required] public string YearLevelName { get; set; }
    [Required] public string YearLevelCode { get; set; }
    public string? Description { get; set; }
    [Required] public bool toDelete { get; set; }
}