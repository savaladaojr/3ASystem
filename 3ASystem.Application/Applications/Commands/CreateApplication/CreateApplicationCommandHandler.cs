using _3ASystem.Application.Abstractions.Data;
using _3ASystem.Application.Abstractions.Messaging;
using _3ASystem.Domain.Data.Repositories;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace _3ASystem.Application.Applications.Commands.CreateApplication;

internal sealed class CreateApplicationCommandHandler : ICommandHandler<CreateApplicationCommand, CreateApplicationResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAppRepository _appRepository;

	public CreateApplicationCommandHandler(IUnitOfWork unitOfWork, IAppRepository appRepository)
	{
		_unitOfWork = unitOfWork;
		_appRepository = appRepository;
	}

	public async Task<Result<CreateApplicationResponse>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
	{
		var app = App.Create(request.Name, request.Abbreviation, request.Description, request.IconUrl);

		app.Raise(new AppCreatedDomainEvent(app.Id));

		_appRepository.Create(app);

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		var finalResult = new CreateApplicationResponse
		{
			Id = app.Id.Value,
			Name = app.Name,
			Abbreviation = app.Abbreviation,
			Description = app.Description,
			IconUrl = app.IconUrl,
			Hash = app.Hash,
			IsActive = app.IsActive
		};

		return finalResult;
	}

}
