using FluentValidation;

namespace _3ASystem.Application.UseCases.Modules.Queries.GetModuleById;

public sealed class GetModulesByApplicationIdValidator : AbstractValidator<GetModulesByApplicationIdQuery>
{
	public GetModulesByApplicationIdValidator()
	{
		RuleFor(c => c.AppId).NotEmpty();
	}

}
