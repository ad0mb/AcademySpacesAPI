using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.HelperFiles;
using Core.ApplicationCore.UseCases;
using Core.Exceptions;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class AnnouncementRepository: ICreateAnnouncementService
{
    private readonly MyDbContext _dbContext;
    private readonly AblyPublisher _ablyPublisher;
   
    public AnnouncementRepository(MyDbContext dbContext )
    {
        _ablyPublisher = new AblyPublisher();
        _dbContext = dbContext;
    }
    public async Task creatandSaveAnnouncementAsync(AnnouncementEntry announcementEntry)
    {
        try
        {
            var newannouncement = new Announcment()
            {
                AnnouncmentId = Guid.NewGuid().ToString(),
                Title = announcementEntry.Title,
                Message = announcementEntry.Message,
                Tags = announcementEntry.Tags,
                Priority = announcementEntry.Priority,
                SchoolId = announcementEntry.SchoolId,
                SenderId = 31,
                CreatedAt = DateTime.Now
            };
            Console.WriteLine($"Creating announcement With Tags: {newannouncement.Tags} at {newannouncement.CreatedAt}");
        
            await _dbContext.Announcements.AddAsync(newannouncement);
            var result = await _dbContext.SaveChangesAsync();
            await _ablyPublisher.BroadcastAnnounccementsAsync(announcementEntry);
            if (result == 0)
            {
                throw new NoRowsAffectedException("Announcement not created");
            }
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue adding announcement to the database", ex);
        }
    }
    // public async task deleteAnnouncementAsync(string id)
    // {
    //     try
    //     {
    //         var announcement = _dbContext.Announcements.FirstOrDefault(a => a.Id == id);
    //         if (announcement == null)
    //         {
    //             throw new NotFoundException("Announcement not found");
    //         }
    //         _dbContext.Announcements.Remove(announcement);
    //         var result = _dbContext.SaveChangesAsync();
    //         if (result.Result == 0)
    //         {
    //             throw new NoRowsAffectedException("Announcement not deleted");
    //         }
    //     }
    //     catch (DbUpdateException ex)
    //     {
    //         throw new DbException("Issue deleting announcement from the database", ex);
    //     }
    // }
}