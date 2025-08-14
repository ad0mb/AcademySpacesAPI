using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.DTOs;

public class PaginatedAnnouncementsDto
{
    public List<AnnouncementEntry> Announcements { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}