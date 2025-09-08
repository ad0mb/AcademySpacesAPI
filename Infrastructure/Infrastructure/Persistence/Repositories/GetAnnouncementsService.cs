using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.DTOs;
using Core.ApplicationCore.Interfaces.UseCases;
using FirebaseAdmin.Messaging;
using Infrastructure.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class GetAnnouncementsService: IGetAnnouncementsUseCase
{
    private readonly MyDbContext _context;
    public GetAnnouncementsService(MyDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedAnnouncementsDto> GetAnnouncementsAsync(int pageNumber, int pageSize,int schoolID )
    {
        
       // Console.WriteLine("GetAnnouncementsAsync");
        var totalAnnouncements = await _context.Announcements.CountAsync();
        
        var infraAnnouncements = await _context.Announcements
            .Where(a => a.SchoolId == schoolID)
            .OrderByDescending(a => a.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(a =>a.Tags)
            .ToListAsync();

        var coreAnnouncements = infraAnnouncements.Select(a => new AnnouncementEntry
        {
            Id = a.AnnouncmentId,
            Title = a.Title,
            Message = a.Message,
            Date = a.CreatedAt,
            SenderId = a.SenderId,
            Priority = a.Priority,
            SchoolId = a.SchoolId,
            IsUrgent = a.Priority == "urgent",
            Tags = a.Tags.Select(t => new TagEntry
            {
                Name = t.Name,
            }).ToList()

        }).ToList();
        Console.WriteLine("GetAnnouncementsAsync");
       

        return new PaginatedAnnouncementsDto
        {
            Announcements = coreAnnouncements,
            TotalCount = totalAnnouncements,
            Page = pageNumber,
            PageSize = totalAnnouncements,
        };
    }
}