using _3ASystem.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Application.Applications.Commands.CreateApplication;

public sealed class CreateApplicationCommand : ICommand<CreateApplicationResponse>
{
	public string Name { get; set; } = string.Empty;
	public string Abbreviation { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string IconUrl { get; set; } = string.Empty;

}
