using FluentValidation;

namespace _3ASystem.Application.UseCases.Applications.Queries.GetApplicationById;

public sealed class GetApplicationByIdQueryValidator : AbstractValidator<GetApplicationByIdQuery>
{
	public GetApplicationByIdQueryValidator()
	{
		RuleFor(c => c.Id).NotEmpty();
	}

}
