using FluentValidation;

namespace _3ASystem.Application.Applications.Queries.GetApplicationById;

public sealed class GetApplicationByIdQueryValidator : AbstractValidator<GetApplicationByIdQuery>
{
	public GetApplicationByIdQueryValidator()
	{
		RuleFor(c => c.Id).NotEqual(Guid.Empty);
	}

}
