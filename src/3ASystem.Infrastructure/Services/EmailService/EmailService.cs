using _3ASystem.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Infrastructure.Services.EmailService;
public class EmailService : IEmailService
{
	
	SMTPConfiguration _smtpConfiguration = new SMTPConfiguration();


	public EmailService(IConfiguration configuration)
	{
		_smtpConfiguration = new SMTPConfiguration
		{
			Host = configuration["SMTPConfiguration:Host"] ?? throw new InvalidOperationException("SMTPConfiguration:Host is not configured."),
			Port = int.TryParse(configuration["SMTPConfiguration:Port"], out var port) ? port : throw new InvalidOperationException("SMTPConfiguration:Port is not configured or invalid."),
			EnableSsl = bool.TryParse(configuration["SMTPConfiguration:EnableSsl"], out var enableSsl) ? enableSsl : throw new InvalidOperationException("SMTPConfiguration:EnableSsl is not configured or invalid."),
			UserName = configuration["SMTPConfiguration:UserName"] ?? throw new InvalidOperationException("SMTPConfiguration:UserName is not configured."),
			Password = configuration["SMTPConfiguration:Password"] ?? throw new InvalidOperationException("SMTPConfiguration:Password is not configured."),
		};

	}

	public Task<bool> SendEmailAsync(string fromEmail, string fromName, string toEmail, string toName, string subject, string body)
	{
		return SendEmailAsync(fromEmail, fromName, toEmail, toName, subject, body, string.Empty);		
	}

	public Task<bool> SendEmailAsync(string fromEmail, string fromName, string toEmail, string toName, string subject, string body, string attachmentPath)
	{
		SmtpClient smtpClient = new SmtpClient(_smtpConfiguration.Host, _smtpConfiguration.Port)
		{
			Credentials = new System.Net.NetworkCredential(_smtpConfiguration.UserName, _smtpConfiguration.Password),
			EnableSsl = _smtpConfiguration.EnableSsl,
			DeliveryMethod = SmtpDeliveryMethod.Network
		};


		var mail = new MailMessage()
		{
			From = new MailAddress(fromEmail, fromName),
			To = { new MailAddress(toEmail, toName) },
			Subject = subject,
			Body = body,
			IsBodyHtml = true
		};

		// If an attachment path is provided, add the attachment
		if (!string.IsNullOrEmpty(attachmentPath))
		{
			Attachment attachment = new Attachment(attachmentPath);
			mail.Attachments.Add(attachment);
		}

		try
		{
			smtpClient.Send(mail);
			return Task.FromResult(true);
		}
		catch (Exception)
		{
			return Task.FromResult(false);
		}


	}

}
