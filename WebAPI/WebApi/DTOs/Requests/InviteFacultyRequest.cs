using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class InviteFacultyRequest
{
    [Required] public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    [Required] public string LastName { get; set; }
    [Phone] public string? PhoneNumber { get; set; }
    [Required] [EmailAddress] public string Email { get; set; } 
    [Required] public bool Invite { get; set; }
}