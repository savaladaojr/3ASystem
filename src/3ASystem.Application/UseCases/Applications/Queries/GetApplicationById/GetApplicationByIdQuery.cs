﻿using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;

namespace _3ASystem.Application.UseCases.Applications.Queries.GetApplicationById;

public sealed class GetApplicationByIdQuery : IQuery<ApplicationDetailedResponse>
{
	public Guid Id { get; set; }

}
