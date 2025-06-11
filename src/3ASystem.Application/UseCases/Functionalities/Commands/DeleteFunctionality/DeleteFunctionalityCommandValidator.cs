using FluentValidation;

namespace _3ASystem.Application.UseCases.Functionalities.Commands.DeleteFunctionality;

public sealed class DeleteFunctionalityCommandValidator : AbstractValidator<DeleteFunctionalityCommand>
{
	public DeleteFunctionalityCommandValidator()
	{
		RuleFor(c => c.Id).NotEmpty();
	}

}