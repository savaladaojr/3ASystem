/*using FluentValidation;

namespace _3ASystem.Domain.Entities.Operations;

public class OperationValidator : AbstractValidator<Operation>
{
    public OperationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(OperationErrors.InvalidName);
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(OperationErrors.InvalidCode);
        RuleFor(x => x.FriendlyId)
            .NotEmpty().WithMessage(OperationErrors.InvalidFriendlyId);
        // Regras adicionais podem ser adicionadas conforme necessário
    }
}
*/