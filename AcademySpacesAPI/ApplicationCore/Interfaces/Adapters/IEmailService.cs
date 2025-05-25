namespace AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string message);
    Task SendEmailAsync(string[] email, string subject, string message);
    Task SendSchoolRegistrationEmailAsync(string email, string token);
    Task SendFacultyRegistrationEmailAsync(string email, string token);
}