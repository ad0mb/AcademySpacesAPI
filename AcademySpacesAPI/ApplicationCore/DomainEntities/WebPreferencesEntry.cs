namespace AcademySpacesAPI.ApplicationCore.DomainEntities;

public class WebPreferencesEntry
{
    public string IdentityId { get; set; }
    public required string PageBrightness { get; set; } = "system";
    public required string Locale { get; set; } = "en";
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}