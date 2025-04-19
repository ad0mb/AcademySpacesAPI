using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.Models.RequestModels;

public class RegisterRequest
{
    [Required] public string SchoolName { get; set; }
    [Required] public string SchoolCountry { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    [Required] public string SigninEmail { get; set; }
    [Required] public string IdentityId { get; set; }
}