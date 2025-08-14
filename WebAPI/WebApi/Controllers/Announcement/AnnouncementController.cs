using AcademySpacesAPI.WebApi.DTOs.Requests;
using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;
using Core.ApplicationCore.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using YourNamespace.DTOs;

namespace AcademySpacesAPI.WebApi.Controllers.Announcement;
[ApiController]
//[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
[Route("api/Announcements")]
public class AnnouncementController:ControllerBase
{
    private readonly ICreateAnnouncementUseCase _createAnnouncementUseCase;
    private readonly IGetAnnouncementsUseCase _getAnnouncementsUseCase;
    private readonly IFacultyRepository _facultyRepository;
    
    public AnnouncementController(
        ICreateAnnouncementUseCase createAnnouncementUseCase,
        IGetAnnouncementsUseCase getAnnouncementsUseCase, 
        IFacultyRepository facultyRepository)
    {
        _facultyRepository = facultyRepository;
        _createAnnouncementUseCase = createAnnouncementUseCase;
        _getAnnouncementsUseCase = getAnnouncementsUseCase;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAnnouncement([FromBody] CreateAnnouncementDto announcementDto)
    {
        

        
            
            // var identityID =  Request.Cookies["access"];
            //
            //  if (string.IsNullOrEmpty(identityID))
            //  {
            //      return Unauthorized(new
            //      {
            //          Status = false,
            //          Message = "User identity could not be determined.",
            //          Data = (object)null,
            //          Errors = new[] { "Missing or invalid token." }
            //      });
            //  }
            //
            
            // Assuming this method retrieves the faculty member based on the current user's identity ID
            var announcement = new AnnouncementEntry()
            {
                Title = announcementDto.Title,
                Message = announcementDto.Message,
                Date = DateTime.Now,
                Tags = announcementDto.Tags,
                SchoolId = 27, // Assuming a static school ID for demonstration
               // Sender = "Me for now", // Assuming a static sender for demonstration
                Priority = announcementDto.IsUrgent ? "urgent" : null
                
            };
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
    [HttpGet ]
    public async Task<IActionResult> GetAnnouncements([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        Console.WriteLine("here");
        var result = await _getAnnouncementsUseCase.GetAnnouncementsAsync(page, pageSize);
        
        return Ok(result);
    }
}