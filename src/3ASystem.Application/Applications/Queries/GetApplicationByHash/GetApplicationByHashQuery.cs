using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.Applications.Shared;

namespace _3ASystem.Application.Applications.Queries.GetApplicationByHash;

public sealed class GetApplicationByHashQuery : IQuery<ApplicationResponse>
{
	public Guid Hash { get; set; }

}
