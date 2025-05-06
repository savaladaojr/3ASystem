using FluentValidation;

namespace _3ASystem.Application.UseCases.Modules.Commands.DeleteModule;

public sealed class DeleteModuleCommandValidator : AbstractValidator<DeleteModuleCommand>
{
	public DeleteModuleCommandValidator()
	{
		RuleFor(c => c.Id).NotEmpty();
	}

}