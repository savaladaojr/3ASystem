using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Data.Repositories;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Entities.Modules;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.UseCases.Modules.Commands.DeleteModule;

public class DeleteModuleCommandHandle : ICommandHandler<DeleteModuleCommand>
{
	private readonly IModuleRepository _moduleRepository;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteModuleCommandHandle(IUnitOfWork unitOfWork, IModuleRepository moduleRepository)
	{
		_moduleRepository = moduleRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
	{
		var moduleId = new ModuleId(request.Id);
		var module = await _moduleRepository.GetByIdAsync(moduleId);

		if (module is null)
			return Result.Failure(ModuleErrors.NotFound(moduleId));

		_moduleRepository.NoTrack(module);
		_moduleRepository.Delete(moduleId);

		module.Raise(new ModuleDeleteDomainEvent(module.Id));

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}
