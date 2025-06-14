using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class UpdateFacultyRequest
{
    [Required] public int FacultyId { get; set; }
    [Required] public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    [Required] public string LastName { get; set; }
    [Phone] public string? PhoneNumber { get; set; }
}