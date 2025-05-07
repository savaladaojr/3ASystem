namespace _3ASystem.Application.Abstractions.Services;

public interface IEmailServices
{
	Task<bool> SendEmailAsync(string to, string subject, string body);
	Task<bool> SendEmailAsync(string to, string subject, string body, string attachmentPath);
}
