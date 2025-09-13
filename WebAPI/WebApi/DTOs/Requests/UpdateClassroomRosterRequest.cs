using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class UpdateClassroomRosterRequest
{
    [Required] public List<int> StudentIds { get; set; }
}