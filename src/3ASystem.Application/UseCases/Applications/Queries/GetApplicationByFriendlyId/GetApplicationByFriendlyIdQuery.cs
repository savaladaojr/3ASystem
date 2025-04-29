using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;

namespace _3ASystem.Application.UseCases.Applications.Queries.GetApplicationByFriendlyId;

public sealed class GetApplicationByFriendlyIdQuery : IQuery<ApplicationResponse>
{
	public string FriendlyId { get; set; } = string.Empty;

}
