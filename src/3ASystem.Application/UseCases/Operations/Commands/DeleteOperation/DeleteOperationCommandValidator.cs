using FluentValidation;

namespace _3ASystem.Application.UseCases.Operations.Commands.DeleteOperation;

public class DeleteOperationCommandValidator : AbstractValidator<DeleteOperationCommand>
{
    public DeleteOperationCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
    }
}
