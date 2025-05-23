using _3ASystem.WebUI.Server.Components.Account.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace _3ASystem.WebUI.Server.Components.Account
{
	public class CookieAuthEvents : CookieAuthenticationEvents
	{
		public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
		{
			context.RedirectUri = "/Account/Login";
			return base.RedirectToLogin(context);
		}

	}
}
