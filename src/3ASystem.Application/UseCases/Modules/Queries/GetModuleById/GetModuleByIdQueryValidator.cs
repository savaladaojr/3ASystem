using FluentValidation;

namespace _3ASystem.Application.UseCases.Modules.Queries.GetModuleById;

public sealed class GetModuleByIdQueryValidator : AbstractValidator<GetModuleByIdQuery>
{
	public GetModuleByIdQueryValidator()
	{
		RuleFor(c => c.Id).NotEmpty();
	}

}
