using _3ASystem.Application.UseCases.Operations.Commands.CreateOperation;
using FluentValidation;

namespace _3ASystem.Application.UseCases.Operations.Commands.AddBasicOperations;

public class AddBasicOperationsCommandValidator : AbstractValidator<AddBasicOperationsCommand>
{
    public AddBasicOperationsCommandValidator()
    {
        RuleFor(p => p.FunctionalityId).NotEmpty();
    }
}
