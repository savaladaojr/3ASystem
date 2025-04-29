using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;

namespace _3ASystem.Application.UseCases.Applications.Queries.GetApplicationByAbbreviation;

public sealed class GetApplicationByAbbreviationQuery : IQuery<ApplicationResponse>
{
	public string Abbreviation { get; set; } = string.Empty;

}
