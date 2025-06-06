using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;

namespace _3ASystem.Application.UseCases.Applications.Queries.GetApplicationsPaged;

public sealed class GetApplicationsPagedQuery : PagedQuery, IQuery<PagedList<ApplicationResponse>>
{

}
