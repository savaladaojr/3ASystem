using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Modules.Responses;

namespace _3ASystem.Application.UseCases.Modules.Queries.GetModulesPaged;

public sealed class GetModulesPagedQuery : PagedQuery, IQuery<PagedList<ModuleResponse>>
{

}
