using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Operations;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Functionalities.Commands.DeleteFunctionality;

public class DeleteFunctionalityCommandHandle : ICommandHandler<DeleteFunctionalityCommand>
{
	private readonly IFunctionalityRepository _functionalityRepository;
	private readonly IOperationRepository _operationRepository;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteFunctionalityCommandHandle(
		IUnitOfWork unitOfWork,
		IFunctionalityRepository functionalityRepository,
		IOperationRepository operationRepository)
	{
		_functionalityRepository = functionalityRepository;
		_operationRepository = operationRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(DeleteFunctionalityCommand request, CancellationToken cancellationToken)
	{
		var functionalityId = new FunctionalityId(request.Id);
		var functionality = await _functionalityRepository.GetByIdAsync(functionalityId);

		if (functionality is null)
			return Result.Failure(FunctionalityErrors.NotFound(functionalityId));

		if (await _operationRepository.ExistsByFunctionalityIdAsync(functionalityId))
			return Result.Failure(OperationErrors.FunctionalityHasOperations(functionalityId));

		_functionalityRepository.NoTrack(functionality);
		_functionalityRepository.Delete(functionalityId);

		functionality.Raise(new FunctionalityDeleteDomainEvent(functionality.Id));

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}
