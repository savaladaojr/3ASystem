using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Modules.Responses;

namespace _3ASystem.Application.UseCases.Modules.Queries.GetModuleById;

public sealed class GetModuleByIdQuery : IQuery<ModuleDetailedResponse>
{
	public Guid Id { get; set; }

}
