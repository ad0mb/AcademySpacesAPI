namespace Core.ApplicationCore.DomainEntities;

public class WebPreferencesEntry
{
    public string IdentityId { get; set; }
    public string? PageBrightness { get; set; }
    public string? Locale { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}