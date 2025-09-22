using System.Net;
using System.Net.Mail;
using Azure;
using Azure.Communication.Email;
using Core.ApplicationCore.Interfaces.Adapters;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Infrastructure.Email;

public class EmailService : IEmailService
{
    
    private readonly EmailClient _emailClient;
    private readonly IConfiguration _configuration;

    public EmailService(EmailClient emailClient, IConfiguration configuration)
    {
        _emailClient = emailClient;
        _configuration = configuration;
    }

    public async Task SendSchoolRegistrationEmailAsync(string email, string tokenString)
    {
        var activationLink =
            $"{_configuration["FrontendDomain"]}/signup/school?token={WebUtility.UrlEncode(tokenString)}";
        
        var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Infrastructure", "Email", "Templates",
            "RegisterSchoolEmailTemplates", "en.html");
        
        var htmlTemplate = await File.ReadAllTextAsync(templatePath);
        htmlTemplate = htmlTemplate.Replace("{{ActivationLink}}", activationLink);
        
        var emailMessage = new EmailMessage(
            senderAddress: _configuration["NoReplyEmail"],
            content: new EmailContent("AcademySpaces School Resgistration")
            {
                Html = htmlTemplate
            },
            recipients: new EmailRecipients(new List<EmailAddress>
            {
                new EmailAddress(email)
            })
        );
        
        var emailSendOperation = await _emailClient.SendAsync(
            WaitUntil.Completed,
            emailMessage);
    }
}

    