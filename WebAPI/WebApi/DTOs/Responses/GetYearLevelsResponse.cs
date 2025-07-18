using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Responses;

public class GetYearLevelsResponse
{
    [Required] public int Id { get; set; }
    [Required] public string YearLevelName { get; set; }
    [Required] public string YearLevelCode { get; set; }
    public string? Description { get; set; }
}