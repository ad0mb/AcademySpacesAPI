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
        

        await _service.creatandSaveAnnouncementAsync(newannouncement);
    }

 
    

   
}