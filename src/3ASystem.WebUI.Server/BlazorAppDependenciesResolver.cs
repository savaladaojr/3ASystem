using _3ASystem.WebUI.Server.Components.Account;
using _3ASystem.WebUI.Server.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

public static class BlazorAppDependenciesResolver
{
	public static IServiceCollection AddAuthenticationAutorizationToBlazorApp(this IServiceCollection builder, IConfiguration configuration)
	{
		builder.AddCascadingAuthenticationState();
		builder.AddScoped<IdentityUserAccessor>();
		builder.AddScoped<IdentityRedirectManager>();
		builder.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

		builder.AddAuthentication(options =>
		{
			options.DefaultScheme = IdentityConstants.ApplicationScheme;
			options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
		})
			.AddIdentityCookies();

		var connectionString = configuration.GetConnectionString("BlazorAppConnection") ?? throw new InvalidOperationException("Connection string 'BlazorAppConnection' not found.");
		builder.AddDbContext<BlazorAppDbContext>(options =>
			options.UseSqlServer(connectionString));
		builder.AddDatabaseDeveloperPageExceptionFilter();

		builder.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddEntityFrameworkStores<BlazorAppDbContext>()
			.AddSignInManager()
			.AddDefaultTokenProviders();

		builder.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();


		//Cookies
		builder.AddScoped<CookieAuthEvents>();
		builder.ConfigureApplicationCookie(options =>
		{
			//options.LoginPath = "/Account/Login";
			//options.LogoutPath = "/Account/Logout";
			//options.AccessDeniedPath = "/Account/AccessDenied";
			options.EventsType = typeof(CookieAuthEvents);
		});

		return builder;
	}
}
