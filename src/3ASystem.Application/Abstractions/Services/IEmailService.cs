namespace _3ASystem.Application.Abstractions.Services;

public interface IEmailService
{
	Task<bool> SendEmailAsync(string toEmail, string subject, string body);
	Task<bool> SendEmailAsync(string fromEmail, string toEmail, string subject, string body);
	
	Task<bool> SendEmailWithNamesAsync(string toEmail, string toName, string subject, string body);
	Task<bool> SendEmailWithNamesAsync(string fromEmail, string fromName, string toEmail, string toName, string subject, string body);

	Task<bool> SendEmailWithAttachmentAsync(string toEmail, string subject, string body, string attachmentPath);
	Task<bool> SendEmailWithAttachmentAsync(string fromEmail, string toEmail, string subject, string body, string attachmentPath);

	Task<bool> SendEmailWithNamesAndAttachmentAsync(string toEmail, string toName, string subject, string body, string attachmentPath);
	Task<bool> SendEmailWithNamesAndAttachmentAsync(string fromEmail, string fromName, string toEmail, string toName, string subject, string body, string attachmentPath);

}
