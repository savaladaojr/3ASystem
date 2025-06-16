using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Application.UseCases.Functionalities.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Functionalities.Commands.EnableDisableFunctionality;

public sealed class EnableDisableFunctionalityCommandHandler : ICommandHandler<EnableDisableFunctionalityCommand, FunctionalityDetailedResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IFunctionalityRepository _functionalityRepository;

	public EnableDisableFunctionalityCommandHandler(IUnitOfWork unitOfWork, IFunctionalityRepository functionalityRepository)
	{
		_unitOfWork = unitOfWork;
		_functionalityRepository = functionalityRepository;
	}

	public async Task<Result<FunctionalityDetailedResponse>> Handle(EnableDisableFunctionalityCommand request, CancellationToken cancellationToken)
	{
		var functionalityId = new FunctionalityId(request.Id);

		var functionality = await _functionalityRepository.GetByIdAsync(functionalityId);


		if (functionality is null)
			return Result.Failure<FunctionalityDetailedResponse>(FunctionalityErrors.NotFound(functionalityId));

		//handle disable/enable
		if (functionality.IsActive)
		{
			functionality.Disable();
			functionality.Raise(new FunctionalityDisabledDomainEvent(functionality.Id));
		}
		else
		{
			functionality.Enable();
			functionality.Raise(new FunctionalityEnabledDomainEvent(functionality.Id));
		}

		//Save changes
		_functionalityRepository.Update(functionality);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		///Return created functionality as response
		return functionality.ToFunctionalityDetailedResponse();
	}

}
