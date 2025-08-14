using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class CreateAnnouncementUseCase: ICreateAnnouncementUseCase
{
    private readonly ICreateAnnouncementService _service;

    public CreateAnnouncementUseCase(ICreateAnnouncementService service)
    {
        _service = service;
    }

    public async Task HandleAnnouncementAsync(AnnouncementEntry newannouncement)
    {
        var announcement = new AnnouncementEntry
        {
            Title = newannouncement.Title,
            Message = newannouncement.Message,
            SenderId = newannouncement.SenderId,
            Tags = newannouncement.Tags,
            SchoolId = newannouncement.SchoolId,
            Date = newannouncement.Date,
            Priority = newannouncement.Priority
        };

        await _service.creatandSaveAnnouncementAsync(announcement);
    }
}