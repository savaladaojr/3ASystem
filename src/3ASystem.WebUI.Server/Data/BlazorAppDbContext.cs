using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _3ASystem.WebUI.Server.Data;
public class BlazorAppDbContext(DbContextOptions<BlazorAppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
}
