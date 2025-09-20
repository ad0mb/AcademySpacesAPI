using System.Net;
using System.Security.Claims;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;
using Core.ApplicationCore.UseCases;
using Infrastructure.Infrastructure.Auth;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using YourNamespace.DTOs;

namespace AcademySpacesAPI.WebApi.Controllers.Announcement;
[ApiController]

[Route("api/Announcements")]
public class AnnouncementController:ControllerBase
{
    private readonly ICreateAnnouncementUseCase _createAnnouncementUseCase;
    private readonly IGetAnnouncementsUseCase _getAnnouncementsUseCase;
    private readonly IFacultyRepository _facultyRepository;
    private readonly AuthService _authService;
    
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public AnnouncementController(
        ICreateAnnouncementUseCase createAnnouncementUseCase,
        IGetAnnouncementsUseCase getAnnouncementsUseCase, 
        IFacultyRepository facultyRepository,
        IHttpContextAccessor httpContextAccessor,
        AuthService authService)
    {
        _httpContextAccessor = httpContextAccessor;
        _facultyRepository = facultyRepository;
        _createAnnouncementUseCase = createAnnouncementUseCase;
        _getAnnouncementsUseCase = getAnnouncementsUseCase;
        _authService = authService;
    }

    
  //This should be done in auth 
    [HttpPost("CreateAnnouncement")]
    public async Task<IActionResult> CreateAnnouncement([FromBody] CreateAnnouncementDto<string> announcementDto)
    {
  
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        string idToken = null;

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            idToken = authHeader.Substring("Bearer ".Length).Trim();
        }
        else
        {
            idToken = Request.Cookies["access"];
        }

        if (string.IsNullOrEmpty(idToken))
            return Unauthorized("Missing token");

        var principal = await _authService.ProcessIdTokenAsync(idToken);


        var identityId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var Id = principal.FindFirst("school_id")?.Value;


        int schoolId = int.Parse(Id);
            
            
            // Assuming this method retrieves the faculty member based on the current user's identity ID
            var announcement = new AnnouncementEntry()
            {
                Title = announcementDto.Title,
                Message = announcementDto.Message,
                Date = DateTime.Now,
                SchoolId =schoolId, // Assuming a static school ID for demonstration
               // Sender = "Me for now", // Assuming a static sender for demonstration
                Priority = announcementDto.IsUrgent ? "urgent" : null
                
            };
            
            Console.WriteLine("This is the School ID:",announcement.SchoolId);
            
            //Here I need to iterate through announcemnetDDto.Tags and save each tag in the DB Through the HandleAnnouncementAsync
            if (announcementDto.Tags != null && announcementDto.Tags.Any())
            {
                foreach (var tag in announcementDto.Tags)
                {
                    announcement.Tags.Add(new TagEntry()
                    {
                         Name = tag,
                    });
                }
            }
            
            //
            // Console.WriteLine("Announcement created with title: " + announcement.Title);
            // Console.WriteLine("Announcement created with Message: " + announcement.Message);
            // Console.WriteLine("Announcement created with Date: " + announcement.Date);
            // Console.WriteLine("Announcement created with Tags: " + announcement.Tags);
            // Console.WriteLine("Announcement created with Priority: " + announcement.Priority);
            // Console.WriteLine(announcementDto.IsUrgent ? "This is an urgent announcement." : "This is not an urgent announcement.");
            // Console.WriteLine("Announcement created with Sender: " + announcement.Sender);
            try
            {
                await _createAnnouncementUseCase.HandleAnnouncementAsync(announcement); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            Console.WriteLine("Announcement created successfully.");
            return Ok();
    }
   

    [HttpGet("GetAnnouncements")]
    public async Task<IActionResult> GetAnnouncements([FromQuery]  int page = 1, [FromQuery] int pageSize = 50)
    {
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        string idToken = null;

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            idToken = authHeader.Substring("Bearer ".Length).Trim();
        }
        else
        {
            idToken = Request.Cookies["access"];
        }

        if (string.IsNullOrEmpty(idToken))
            return Unauthorized("Missing token");

        var principal = await _authService.ProcessIdTokenAsync(idToken);


        var identityId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var Id = principal.FindFirst("school_id")?.Value;

        Console.WriteLine(Id);
        int schoolId = int.Parse(Id);
        var result = await _getAnnouncementsUseCase.GetAnnouncementsAsync(page, pageSize,schoolId);
        
        return Ok(result);
    }
}