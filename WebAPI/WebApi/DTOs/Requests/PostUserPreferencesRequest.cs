using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class PostUserPreferencesRequest
{
    public string? PageBrightness { get; set; }
    public string? Locale { get; set; }
}