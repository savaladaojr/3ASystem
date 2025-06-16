using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Functionalities.Responses;

namespace _3ASystem.Application.UseCases.Functionalities.Queries.GetFunctionalitiesPaged;

public sealed class GetFunctionalitiesPagedQuery : PagedQuery, IQuery<PagedList<FunctionalityResponse>>
{

}
