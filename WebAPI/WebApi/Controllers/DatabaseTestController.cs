using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Infrastructure.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.RateLimiting;


namespace AcademySpacesAPI.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseTestController : ControllerBase
    {
        private readonly MyDbContext _context;

        public DatabaseTestController(MyDbContext context)
        {
            _context = context;
        }
        [EnableRateLimiting("fixed")] 
        [HttpGet("ping")]
        public async Task<IActionResult> PingDatabase()
        {
            try
            {
                // Simple DB check
                var canConnect = await _context.Database.CanConnectAsync();

                if (canConnect)
                {
                    return Ok(new
                    {
                        status = "success",
                        message = "Database connection successful."
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        status = "error",
                        message = "Database connection failed."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "An error occurred while testing the database connection.",
                    details = ex.Message
                });
            }
        }
    }
}