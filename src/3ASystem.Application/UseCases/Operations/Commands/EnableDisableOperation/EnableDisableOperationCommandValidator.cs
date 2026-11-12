using FluentValidation;

namespace _3ASystem.Application.UseCases.Operations.Commands.EnableDisableOperation;

public class EnableDisableOperationCommandValidator : AbstractValidator<EnableDisableOperationCommand>
{
    public EnableDisableOperationCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
    }
}
