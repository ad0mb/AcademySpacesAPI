using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class CreatePermissionRequest
{
    public int Id { get; set; }
    [Required] public bool View { get; set; } = false;
    [Required] public bool Create { get; set; } = false;
    [Required] public bool Delete { get; set; } = false;
    [Required] public bool Update { get; set; } = false;

}