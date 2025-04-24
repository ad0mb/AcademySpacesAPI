using AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;
using AcademySpacesAPI.Infrastructure.Firebase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.WebApi.Controllers
{
    [ApiController]
    // [Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly FirebaseAuthService _firebaseAuthService;
        private readonly IFacultyRepository _facultyRepository;

        public UserController(FirebaseAuthService firebaseAuthService, IFacultyRepository facultyRepository)
        {
            _firebaseAuthService = firebaseAuthService;
            _facultyRepository = facultyRepository;
        }

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