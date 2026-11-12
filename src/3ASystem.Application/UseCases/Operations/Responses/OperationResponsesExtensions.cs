using _3ASystem.Application.UseCases.Functionalities.Responses;
using _3ASystem.Domain.Entities.Operations;

namespace _3ASystem.Application.UseCases.Operations.Responses;

public static class OperationResponsesExtensions
{
    public static OperationResponse ToOperationResponse(this Operation operation)
    {
        return new OperationResponse
        {
            Id = operation.Id.Value,
            FunctionalityId = operation.FunctionalityId.Value,
            Name = operation.Name,
            Code = operation.Code,
            FriendlyId = operation.FriendlyId,
            IsActive = operation.IsActive,
            CreatedAt = operation.CreatedAt,
            LastUpdatedAt = operation.LastUpdatedAt,
        };
    }

    public static OperationDetailedResponse ToOperationDetailedResponse(this Operation operation)
    {
        return new OperationDetailedResponse
        {
            Id = operation.Id.Value,
            FunctionalityId = operation.FunctionalityId.Value,
            Name = operation.Name,
            Code = operation.Code,
            FriendlyId = operation.FriendlyId,
            IsActive = operation.IsActive,
            CreatedAt = operation.CreatedAt,
            LastUpdatedAt = operation.LastUpdatedAt,
        };
    }

    public static OperationDetailedResponse ToOperationDetailedResponseWithFunctionality(this Operation operation)
    {
        return new OperationDetailedResponse
        {
            Id = operation.Id.Value,
            FunctionalityId = operation.FunctionalityId.Value,
            Name = operation.Name,
            Code = operation.Code,
            FriendlyId = operation.FriendlyId,
            IsActive = operation.IsActive,
            CreatedAt = operation.CreatedAt,
            LastUpdatedAt = operation.LastUpdatedAt,
            Functionality = operation.Functionality?.ToFunctionalityDetailedResponse(),
        };
    }
}
