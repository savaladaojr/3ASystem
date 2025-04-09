using _3ASystem.Application.Applications.Commands.DeleteApplication;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Application.Applications.Queries.GetApplicationById;

public sealed class GetApplicationByIdQueryValidator : AbstractValidator<GetApplicationByIdQuery>
{
	public GetApplicationByIdQueryValidator()
	{
		RuleFor(c => c.Id).NotEqual(Guid.Empty);
	}

}
