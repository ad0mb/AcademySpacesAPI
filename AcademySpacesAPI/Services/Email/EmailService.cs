using System.Net;
using System.Net.Mail;

namespace AcademySpacesAPI.Services.Email;

public class EmailService
{
    
    private readonly IConfiguration _configuration;
    private readonly SmtpClient _smtpClient;
    
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        _smtpClient = new SmtpClient(_configuration["SmtpNoReply:Host"], int.Parse(_configuration["SmtpNoReply:Port"]))
        {
            // EnableSsl = bool.Parse(_configuration["SmtpNoReply:EnableSsl"]),
            Credentials = new NetworkCredential(_configuration["SmtpNoReply:Email"], _configuration["SmtpNoReply:Password"]),
        };
    }

    //TODO: Fix issue with sender name being staging and not AcademySpaces
    public void SendAdminSchoolRegistration(string email, string token)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["SmtpNoReply:Email"]),
            Subject = "Academy Spaces Registration",
            Body =
                $"You have been invited to register as an admin for Academy Spaces. Please click the link below to complete your registration. http://localhost:3000/signup/school?token={token}",
            IsBodyHtml = true,
        };

        mailMessage.To.Add(email);

        _smtpClient.Send(mailMessage);
    }
}