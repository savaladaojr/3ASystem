using _3ASystem.Application.UseCases.Applications.Responses;
using _3ASystem.Domain.Entities.Applications;
using _3ASystem.Domain.Entities.Modules;

namespace _3ASystem.Application.UseCases.Modules.Responses;
public static class ModuleResponsesExtensions
{
	public static ModuleDetailedResponse ToModuleDetailedResponse(this Module module)
	{
		return new ModuleDetailedResponse
		{
			Id = module.Id.Value,
			ApplicationId = module.ApplicationId.Value,
			Name = module.Name,
			Abbreviation = module.Abbreviation,
			Description = module.Description,
			IconUrl = module.IconUrl,
			FriendlyId = module.FriendlyId,
			IsActive = module.IsActive,
			IsPartOfMenu = module.IsPartOfMenu,
			CreatedAt = module.CreatedAt,
			LastUpdatedAt = module.LastUpdatedAt
		};

	}

	public static ModuleResponse ToModuleResponse(this Module module)
	{

		return new ModuleResponse
		{
			Id = module.Id.Value,
			Name = module.Name,
			Abbreviation = module.Abbreviation,
			IconUrl = module.IconUrl,
			FriendlyId = module.FriendlyId,
			IsActive = module.IsActive,
		};

	}

	public static ModuleResponse ToModuleResponseWithApplication(this Module module)
	{

		return new ModuleResponse
		{
			Id = module.Id.Value,
			Name = module.Name,
			Abbreviation = module.Abbreviation,
			IconUrl = module.IconUrl,
			FriendlyId = module.FriendlyId,
			IsActive = module.IsActive,

			Application = new ApplicationResponse
			{
				Id = module.Application!.Id.Value,
				Name = module.Application.Name,
				Abbreviation = module.Application.Abbreviation,
				IconUrl = module.Application.IconUrl,
				FriendlyId = module.Application.FriendlyId,
				IsActive = module.Application.IsActive
			}
		};

	}


	public static IEnumerable<ModuleResponse> ToIEnumerableOfModuleResponse(this IEnumerable<Module> modules)
	{
		return modules.Select(module => module.ToModuleResponse());
	}

	public static IEnumerable<ModuleResponse> ToIEnumerableOfModuleResponseWithApplication(this IEnumerable<Module> modules)
	{
		return modules.Select(module => module.ToModuleResponseWithApplication());
	}


}
