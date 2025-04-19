using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AcademySpacesAPI.Models
{
    //DO NOT USE THIS CLASS EXCEPT AS A REFERENCE
    public class ReferenceModel
    {
        [Required] public int UserId { get; set; }
        [Required] public int SchoolId { get; set; }
        [Required] public string ProviderId { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }

        [Required]
        [AllowedValues("student", "faculty", "parent",
            ErrorMessage = ("Only faculty, parent, and student types are allowed"))]
        public string UserType { get; set; }
    }
}