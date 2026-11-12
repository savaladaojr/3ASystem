using FluentValidation;

namespace _3ASystem.Application.UseCases.Operations.Commands.CreateOperation;

public class CreateOperationCommandValidator : AbstractValidator<CreateOperationCommand>
{
    public CreateOperationCommandValidator()
    {
        RuleFor(p => p.FunctionalityId).NotEmpty();
        RuleFor(p => p.Name).NotEmpty().MaximumLength(100);
        RuleFor(p => p.Code).NotEmpty().MaximumLength(50);
        RuleFor(p => p.FriendlyId).NotEmpty().MaximumLength(50);
    }
}
