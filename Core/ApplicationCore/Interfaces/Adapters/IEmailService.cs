namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IEmailService
{
    Task SendSchoolRegistrationEmailAsync(string email, string tokenString);
}