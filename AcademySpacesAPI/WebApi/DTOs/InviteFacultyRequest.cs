using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs;

public class InviteFacultyRequest
{
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    [Phone] public string? PhoneNumber { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
}