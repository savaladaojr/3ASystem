using _3ASystem.Application.Abstractions.Services;
using _3ASystem.WebUI.Server.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace _3ASystem.WebUI.Server.Components.Account;

public class IdentityEmailSender : IEmailSender<ApplicationUser>
{
	private readonly IEmailService _emailService;

	public IdentityEmailSender(IEmailService emailService)
	{
		_emailService = emailService;
	}


	public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
	{
		await _emailService.SendEmailAsync(email,
			"Confirm your email",
			$"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>."
		);
	}

	public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
	{
		await _emailService.SendEmailAsync(email,
			"Reset your password",
			$"Please reset your password using the following code: {resetCode}"
		);
	}

	public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
	{
		await _emailService.SendEmailAsync(email,
			"Reset your password",
			$"Please reset your password by <a href='{resetLink}'>clicking here</a>."
		);
	}
}
