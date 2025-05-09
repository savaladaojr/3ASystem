namespace _3ASystem.Application.Abstractions.Services;

public interface IEmailService
{
	Task<bool> SendEmailAsync(string fromEmail, string fromName, string toEmail, string toName, string subject, string body);
	Task<bool> SendEmailAsync(string fromEmail, string fromName, string toEmail, string toName, string subject, string body, string attachmentPath);
}
