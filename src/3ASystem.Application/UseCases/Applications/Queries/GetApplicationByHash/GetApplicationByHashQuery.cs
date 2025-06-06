using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;

namespace _3ASystem.Application.UseCases.Applications.Queries.GetApplicationByHash;

public sealed class GetApplicationByHashQuery : IQuery<ApplicationDetailedResponse>
{
	public Guid Hash { get; set; }

}
