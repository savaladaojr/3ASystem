using FluentValidation;

namespace _3ASystem.Application.UseCases.Modules.Commands.EnableDisableModule;

public class EnableDisableModuleCommandValidator : AbstractValidator<EnableDisableModuleCommand>
{
	public EnableDisableModuleCommandValidator()
	{
		RuleFor(c => c.Id).NotEmpty();
	}

}
