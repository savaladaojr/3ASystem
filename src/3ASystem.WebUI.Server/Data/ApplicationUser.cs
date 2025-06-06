using Microsoft.AspNetCore.Identity;

namespace _3ASystem.WebUI.Server.Data;
// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
	public string FullName { get; set; } = string.Empty;
}

