using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Responses;

public class GetUserPreferencesResponse
{
    [Required] public string PageBrightness { get; set; } = "system";
    [Required] public string Locale { get; set; } = "en";
}