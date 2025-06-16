using FluentValidation;

namespace _3ASystem.Application.UseCases.Functionalities.Commands.EnableDisableFunctionality;

public class EnableDisableFunctionalityCommandValidator : AbstractValidator<EnableDisableFunctionalityCommand>
{
	public EnableDisableFunctionalityCommandValidator()
	{
		RuleFor(c => c.Id).NotEmpty();
	}

}
