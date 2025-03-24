using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Domain.Data.Repositories;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;

namespace _3ASystem.Application.Applications.Commands.DeleteApplication
{
	internal class DeleteApplicationsCommandHandle : ICommandHandler<DeleteApplicationsCommand>
	{
		private readonly IAppRepository _appRepository;
		private readonly IUnitOfWork _unitOfWork;

		public DeleteApplicationsCommandHandle(IAppRepository appRepository, IUnitOfWork unitOfWork)
		{
			_appRepository = appRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(DeleteApplicationsCommand request, CancellationToken cancellationToken)
		{
			var appId = new AppId(request.Id);
			var app = await _appRepository.GetByIdAsync(appId);

			if (app is null)
			{
				return Result.Failure(AppErrors.NotFound(appId));
			}

			_appRepository.NoTrack(app);
			_appRepository.Delete(appId);

			app.Raise(new AppDeleteDomainEvent(app.Id));

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
