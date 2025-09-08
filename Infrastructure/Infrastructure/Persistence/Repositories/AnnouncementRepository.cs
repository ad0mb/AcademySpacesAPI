using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Azure.Core;
using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.HelperFiles;
using Core.ApplicationCore.UseCases;
using Core.Exceptions;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        Console.WriteLine(announcementEntry.SchoolId);
        try
        {
            var newannouncement = new Announcement()
            {
                AnnouncmentId = Guid.NewGuid().ToString(),
                Title = announcementEntry.Title,
                Message = announcementEntry.Message,
                Priority = announcementEntry.Priority,
                SchoolId = announcementEntry.SchoolId,
                SenderId = 31,
                CreatedAt = DateTime.Now
            };
            //Console.WriteLine($"Creating announcement With Tags: {newannouncement.Tags} at {newannouncement.CreatedAt}");
        
           
//Here i want to iterate through each tag and save To DB of the Table i just Gave But I dont now How i should set this up so that tags table and announcement table and announcment_tags are connected and refer correctly 
            foreach (var tag in announcementEntry.Tags)
            {
                var checkExistingtag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Name == tag.Name);

                if (checkExistingtag != null)
                {
                    newannouncement.Tags.Add(checkExistingtag);
                }
                else
                {
                    var newTag = new Tag
                    {
                        Name = tag.Name,
                     
                    };
                    newannouncement.Tags.Add(newTag);
                }
            }
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