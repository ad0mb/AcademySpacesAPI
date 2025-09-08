namespace Core.ApplicationCore.DomainEntities;

public class AnnouncementEntry
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public DateTime Date { get; set; }
    public ICollection<TagEntry> Tags { get; set; } = new HashSet<TagEntry>();
    public int SenderId { get; set; } = default!;
    public int SchoolId { get; set; }
    public string? Priority { get; set; }

    public bool IsUrgent { get;set; } = false;
    //public virtual FacultyEntry? sender { get; set; }
}