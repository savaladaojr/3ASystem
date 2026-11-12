using FluentValidation;

namespace _3ASystem.Application.UseCases.Operations.Commands.UpdateOperation;

public class UpdateOperationCommandValidator : AbstractValidator<UpdateOperationCommand>
{
    public UpdateOperationCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
        RuleFor(p => p.Name).NotEmpty().MaximumLength(100);
        RuleFor(p => p.Code).NotEmpty().MaximumLength(50);
        RuleFor(p => p.FriendlyId).NotEmpty().MaximumLength(50);
    }
}
