namespace Core.ApplicationCore.DomainEntities;

public class WebPreferencesEntry
{
    public string IdentityId { get; set; }
    public string PageBrightness { get; set; } = "system";
    public string Locale { get; set; } = "en";
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}