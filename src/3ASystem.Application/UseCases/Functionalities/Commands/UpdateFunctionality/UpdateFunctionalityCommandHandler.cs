using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Application.UseCases.Functionalities.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Entities.Functionalities;
using _3ASystem.Domain.Entities.Modules;
using _3ASystem.Domain.Shared;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace _3ASystem.Application.UseCases.Functionalities.Commands.UpdateFunctionality;

public sealed class UpdateFunctionalityCommandHandler : ICommandHandler<UpdateFunctionalityCommand, FunctionalityDetailedResponse>
{
	private readonly IFunctionalityRepository _functionalityRepository;
	private readonly IModuleRepository _moduleRepository;
	private readonly IUnitOfWork _unitOfWork;

	public UpdateFunctionalityCommandHandler(IUnitOfWork unitOfWork, IFunctionalityRepository functionalityRepository, IModuleRepository moduleRepository)
	{
		_functionalityRepository = functionalityRepository;
		_moduleRepository = moduleRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<FunctionalityDetailedResponse>> Handle(UpdateFunctionalityCommand request, CancellationToken cancellationToken)
	{
		var functionalityId = new FunctionalityId(request.Id);
		var functionality = await _functionalityRepository.GetByIdAsync(functionalityId);

		if (functionality is null)
			return Result.Failure<FunctionalityDetailedResponse>(FunctionalityErrors.NotFound(functionalityId));

		//Check if the Abbreviation is unique
		var appAbbreviation = await _functionalityRepository.GetByAbbreviationAsync(request.Abbreviation);
		if ( appAbbreviation is not null && appAbbreviation.Id != functionality.Id)
			return Result.Failure<FunctionalityDetailedResponse>(FunctionalityErrors.AbbreviationNotUnique);

		//Check if the FriendlyID is unique
		var appFriendlyId = await _functionalityRepository.GetByFriendlyIdAsync(request.FriendlyId);
		if (appFriendlyId is not null && appFriendlyId.Id != functionality.Id)
			return Result.Failure<FunctionalityDetailedResponse>(FunctionalityErrors.FriendlyIdNotUnique);

		//Update the Functionality
		functionality.Update(request.Name, request.Abbreviation, request.Route, request.IconUrl, request.FriendlyId, request.IsPartOfMenu);

		functionality.Raise(new FunctionalityUpdateDomainEvent(functionality.Id));

		_functionalityRepository.Update(functionality);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		//return updated app
		return functionality.ToFunctionalityDetailedResponse();
	}

}
