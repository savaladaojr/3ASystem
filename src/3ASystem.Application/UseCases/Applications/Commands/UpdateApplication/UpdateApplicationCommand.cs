using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Application.UseCases.Applications.Commands.UpdateApplication
{
	public sealed class UpdateApplicationCommand : ICommand<ApplicationResponse>
	{
		public Guid Id { get; set; } = default!;
		public string Name { get; set; } = string.Empty;
		public string Abbreviation { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string IconUrl { get; set; } = string.Empty;
		public bool IsActive { get; set; } = default!;
		public string FriendlyId { get; set; } = string.Empty;

	}
}
