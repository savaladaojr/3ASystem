using _3ASystem.Application.Abstractions.Messaging;

namespace _3ASystem.Application.Applications.Queries.GetApplicationById;

public class GetApplicationByIdQuery : IQuery<ApplicationResponse>
{
	public Guid Id { get; set; }

}
