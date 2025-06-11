using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Functionalities.Responses;

namespace _3ASystem.Application.UseCases.Functionalities.Queries.GetFunctionalityById;

public sealed class GetFunctionalityByIdQuery : IQuery<FunctionalityDetailedResponse>
{
	public Guid Id { get; set; }

}
