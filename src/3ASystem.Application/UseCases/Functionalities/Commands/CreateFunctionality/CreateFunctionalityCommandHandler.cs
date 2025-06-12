using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Functionalities.Responses;
using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Modules;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Functionalities.Commands.CreateFunctionality;
public class CreateFunctionalityCommandHandler : ICommandHandler<CreateFunctionalityCommand, FunctionalityDetailedResponse>
{
	private readonly IFunctionalityRepository _functionalityRepository;
	private readonly IModuleRepository _moduleRepository;
	private readonly IUnitOfWork _unitOfWork;
	
	public CreateFunctionalityCommandHandler(IFunctionalityRepository functionalityRepository, IModuleRepository repository, IUnitOfWork unitOfWork)
	{
		_functionalityRepository = functionalityRepository;
		_moduleRepository = repository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<FunctionalityDetailedResponse>> Handle(CreateFunctionalityCommand request, CancellationToken cancellationToken)
	{
		//Check if the Abbreviation is unique
		var appAbbreviation = await _functionalityRepository.GetByAbbreviationAsync(request.Abbreviation);
		if (appAbbreviation is not null)
			return Result.Failure<FunctionalityDetailedResponse>(FunctionalityErrors.AbbreviationNotUnique);

		//Check if the FriendlyID is unique
		var appFriendlyId = await _functionalityRepository.GetByFriendlyIdAsync(request.FriendlyId);
		if (appFriendlyId is not null)
			return Result.Failure<FunctionalityDetailedResponse>(FunctionalityErrors.FriendlyIdNotUnique);

		//Check if the Module exists
		var moduleId = new ModuleId(request.ModuleId);
		var module = await _moduleRepository.GetByIdAsync(moduleId);
		if (module is null)
			return Result.Failure<FunctionalityDetailedResponse>(ModuleErrors.NotFound(moduleId));

		//Create the Functionality
		var record = Functionality.Create(moduleId, request.Name, request.Abbreviation, request.Route, request.IconUrl, request.FriendlyId, request.IsPartOfMenu);

		record.Raise(new FunctionalityCreatedDomainEvent(record.Id));

		var functionality = _functionalityRepository.Create(record);

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		//Return created functionality as response
		return functionality.ToFunctionalityDetailedResponse();

	}
}
