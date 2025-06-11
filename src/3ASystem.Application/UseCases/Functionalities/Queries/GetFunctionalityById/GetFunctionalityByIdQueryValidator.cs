using _3ASystem.Application.UseCases.Modules.Queries.GetModuleById;
using FluentValidation;

namespace _3ASystem.Application.UseCases.Functionalities.Queries.GetFunctionalityById;

public sealed class GetFunctionalityByIdQueryValidator : AbstractValidator<GetFunctionalityByIdQuery>
{
	public GetFunctionalityByIdQueryValidator()
	{
		RuleFor(c => c.Id).NotEmpty();
	}

}
