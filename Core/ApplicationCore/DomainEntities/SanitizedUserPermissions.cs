using System.ComponentModel.DataAnnotations;

namespace Core.ApplicationCore.DomainEntities;

public class SanitizedUserPermissions
{
    [Required] public string Name { get; set; }
    [Required] public string Create { get; set; }
    [Required] public string Delete { get; set; }
    [Required] public string Update { get; set; }
}