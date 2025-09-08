using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface ICreateAnnouncementUseCase
{
    Task HandleAnnouncementAsync(AnnouncementEntry newannouncement);
    

}