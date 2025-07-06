using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Responses;

public class GetParentsResponse
{
    [Required] public int ParentId { get; set; }
    [Required] public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    [Required] public string LastName { get; set; }
    [Required] [Phone] public string PhoneNumber { get; set; }
    [EmailAddress] public string Email { get; set; }
}