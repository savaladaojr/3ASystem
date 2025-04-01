using FluentValidation;

namespace _3ASystem.Application.Applications.Commands.DeleteApplication;

public sealed class DeleteApplicationsCommandValidator : AbstractValidator<DeleteApplicationsCommand>
{
	public DeleteApplicationsCommandValidator()
	{
		RuleFor(c => c.Id).NotEqual(Guid.Empty);
	}

}