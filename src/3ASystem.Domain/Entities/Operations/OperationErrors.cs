using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Domain.Entities.Operations;

public static class OperationErrors
{
    public static Error NotFound(OperationId operationId) =>
        Error.NotFound("Operations.NotFound", $"The operation with the Id = '{operationId.Value}' was not found");

    public static Error FunctionalityHasOperations(FunctionalityId functionalityId) =>
        Error.Conflict(
            "Operations.FunctionalityHasOperations",
            $"The functionality with the Id = '{functionalityId.Value}' cannot be deleted while it has operations.");

    public static readonly Error CodeNotUnique = Error.Conflict(
        "Operations.CodeNotUnique",
        "The provided Code already exist.");

    public static readonly Error FriendlyIdNotUnique = Error.Conflict(
        "Operations.FriendlyIdNotUnique",
        "The provided FriendlyId already exist.");
}
