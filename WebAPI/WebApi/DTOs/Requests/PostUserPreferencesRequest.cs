using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class PostUserPreferencesRequest
{
    public string PageBrightness { get; set; } = "system";
    public string Locale { get; set; } = "en";
}