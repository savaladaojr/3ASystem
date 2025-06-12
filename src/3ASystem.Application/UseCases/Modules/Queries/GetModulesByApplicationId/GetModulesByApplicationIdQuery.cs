using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Modules.Responses;

namespace _3ASystem.Application.UseCases.Modules.Queries.GetModuleById;

public sealed class GetModulesByApplicationIdQuery : IQuery<List<ModuleResponse>>
{
	public Guid AppId { get; set; }

}
