using AcademySpacesAPI.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        
        [HasPermission("Students:delete")]
        [HasPermission("Classes:create")]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            var str = "hey";
            Console.WriteLine(str);
            
            // Simulate a delay for testing purposes
            await Task.Delay(1000);
            return Ok(new
            {
                Status = true,
                Message = "Test successful.",
                Data = new { str },
                Errors = (string[])null
            });
        }
    }
}