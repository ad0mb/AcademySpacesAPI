using System.Net;
using System.Net.Mail;
using AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;

namespace AcademySpacesAPI.Infrastructure.Email;

public class EmailService : IEmailService
{

    private readonly IConfiguration _configuration;
    private readonly SmtpClient _smtpClient;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        _smtpClient = new SmtpClient(_configuration["SmtpNoReply:Host"], int.Parse(_configuration["SmtpNoReply:Port"]))
        {
            // EnableSsl = bool.Parse(_configuration["SmtpNoReply:EnableSsl"]),
            Credentials = new NetworkCredential(_configuration["SmtpNoReply:Email"],
                _configuration["SmtpNoReply:Password"]),
        };
    }

    public Task SendEmailAsync(string email, string subject, string message)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["SmtpNoReply:Email"]),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(email);

        return _smtpClient.SendMailAsync(mailMessage);
    }

    public Task SendEmailAsync(string[] email, string subject, string message)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["SmtpNoReply:Email"]),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };

        foreach (var e in email)
        {
            mailMessage.To.Add(e);
        }

        return _smtpClient.SendMailAsync(mailMessage);
    }

    //TODO: Fix issue with sender name being staging and not AcademySpaces
    public Task SendSchoolRegistrationEmailAsync(string email, string token)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["SmtpNoReply:Email"]),
            Subject = "Academy Spaces Registration",
            Body =
                $"You have been invited to register as a school for Academy Spaces. Please click the link below to complete your registration. http://localhost:3000/signup/school?token={token}",
            IsBodyHtml = true,
        };

        mailMessage.To.Add(email);

        return _smtpClient.SendMailAsync(mailMessage);
    }
    
    public Task SendFacultyRegistrationEmailAsync(string email, string token)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["SmtpNoReply:Email"]),
            Subject = "Academy Spaces Registration",
            Body =
                $"You have been invited to register as a faculty for Academy Spaces. Please click the link below to complete your registration. http://localhost:3000/signup/faculty?token={token}",
            IsBodyHtml = true,
        };

        mailMessage.To.Add(email);

        return _smtpClient.SendMailAsync(mailMessage);
    }
}

    