using AcademySpacesAPI.Infrastructure.Firebase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.WebApi.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly FirebaseAuthService _firebaseAuthService;

        public UserController(FirebaseAuthService firebaseAuthService)
        {
            _firebaseAuthService = firebaseAuthService;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            
            
            
            return Ok(new
            {
                Status = true,
                Message = "Test successful.",
                Data = (object)null,
                Errors = (string[])null
            });
        }
    }
}