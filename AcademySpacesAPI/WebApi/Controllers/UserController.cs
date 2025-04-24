using AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;
using AcademySpacesAPI.Infrastructure.Auth;
using AcademySpacesAPI.WebApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.WebApi.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IFacultyRepository _facultyRepository;

        public UserController(AuthService authService, IFacultyRepository facultyRepository)
        {
            _authService = authService;
            _facultyRepository = facultyRepository;
        }

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