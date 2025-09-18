using AcademySpacesAPI.WebApi.Attributes;
using Core.ApplicationCore.Interfaces.Adapters;
using Infrastructure.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AcademySpacesAPI.WebApi.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
    [Route("api/user")]
    public class TestController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IFacultyRepository _facultyRepository;

        public TestController(AuthService authService, IFacultyRepository facultyRepository)
        {
            _authService = authService;
            _facultyRepository = facultyRepository;
        }
        [EnableRateLimiting("fixed")] 
        [HasPermission("User:read")]
        [HttpGet("test")]
        public IActionResult Test()
        {
            
            return Ok(new
            {
                Status = true,
                Message = "Test successful.",
                Data = new {},
                Errors = (string[])null
            });
        }
    }
}