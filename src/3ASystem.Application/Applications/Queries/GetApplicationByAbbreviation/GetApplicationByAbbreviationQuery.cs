using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.Applications.Shared;

namespace _3ASystem.Application.Applications.Queries.GetApplicationById;

public sealed class GetApplicationByAbbreviationQuery : IQuery<ApplicationResponse>
{
	public string Abbreviation { get; set; } = string.Empty;

}
