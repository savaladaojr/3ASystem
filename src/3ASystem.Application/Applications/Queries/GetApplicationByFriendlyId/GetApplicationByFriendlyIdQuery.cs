using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.Applications.Shared;

namespace _3ASystem.Application.Applications.Queries.GetApplicationByFriendlyId;

public sealed class GetApplicationByFriendlyIdQuery : IQuery<ApplicationResponse>
{
	public string FriendlyId { get; set; } = string.Empty;

}
