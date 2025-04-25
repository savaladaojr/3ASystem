using FluentValidation;

namespace _3ASystem.Application.Applications.Commands.DeleteApplication;

public sealed class DeleteApplicationCommandValidator : AbstractValidator<DeleteApplicationCommand>
{
	public DeleteApplicationCommandValidator()
	{
		RuleFor(c => c.Id).NotEmpty();
	}

}