using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Infrastructure.Services.EmailService;

public class SMTPConfiguration
{
	public string Host { get; set; } = string.Empty;
	public int Port { get; set; }
	public bool EnableSsl { get; set; }
	public string UserName { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public string FromEmail { get; set; } = string.Empty;
	public string FromName { get; set; } = string.Empty;
}
