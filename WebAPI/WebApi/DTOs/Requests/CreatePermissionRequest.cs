using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class CreatePermissionRequest
{
    [Required] public bool View { get; set; }
    [Required] public bool Create { get; set; }
    [Required] public bool Delete { get; set; }
    [Required] public bool Update { get; set; }

}