using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.ApplicationCore.Interfaces.Adapters;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;

namespace AcademySpacesAPI.WebApi.Controllers.Developers;

[ApiController]
[Route("api/developers/invite")]
public class DevInviteController : ControllerBase
{
    
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly FirebaseAuth _authService;

    //TODO: Add special auth scheme for dev collection of endpoints
    public DevInviteController(IConfiguration configuration, IEmailService emailService, FirebaseAuth firebaseAuth)
    {
        _configuration = configuration;
        _emailService = emailService;
        _authService = firebaseAuth;
    }
    
    [EnableRateLimiting("fixed")] 
    [HttpPost("invite-admin/{email}")]
    public IActionResult InviteAdmin(string email)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtBearer:Issuer"],
            audience: _configuration["JwtBearer:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(int.Parse(_configuration["JwtBearer:SchoolRegistration:ExpiryInHours"])),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtBearer:SchoolRegistration:Key"])), SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        
        _emailService.SendSchoolRegistrationEmailAsync(email, tokenString);
        
        return Ok(new
        {
            Status = true,
            Message = "Invitation sent successfully.",
            Data = new { email },
            Errors = (string[])null
        });
    }
    
    
    [EnableRateLimiting("fixed")] 
    [HttpPost("add-claim")]
    public async Task<IActionResult> AddClaim(string identityId, string claimType, string claimValue)
    {
        // await _authService.SetCustomUserClaimsAsync(identityId, new Dictionary<string, object>
        // {
        //     { claimType, claimValue }
        // });

        var userRecord = await _authService.GetUserAsync(identityId);
        Console.WriteLine(userRecord.CustomClaims.ToString());

        return Ok(new
        {
            Status = true,
            Message = "Claim added successfully.",
            Data = new { identityId, claimType, claimValue },
            Errors = (string[])null
        });
    }
}