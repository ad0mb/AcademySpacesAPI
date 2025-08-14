using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface ICreateAnnouncementService
{
    Task creatandSaveAnnouncementAsync(AnnouncementEntry announcementEntry);
}