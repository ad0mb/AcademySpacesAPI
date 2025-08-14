using Core.ApplicationCore.DomainEntities;
namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IAblyService
{
    Task BroadcastAnnounccementsAsync(AnnouncementEntry announcements);
}