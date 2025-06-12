using _3ASystem.Application.UseCases.Modules.Responses;
using _3ASystem.Domain.Entities.Functionalities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Application.UseCases.Functionalities.Responses
{
	public static class FunctionalityResponsesExtensions
	{
		public static FunctionalityDetailedResponse ToFunctionalityDetailedResponse(this Functionality functionality)
		{
			return new FunctionalityDetailedResponse()
			{
				Id = functionality.Id.Value,
				ModuleId = functionality.ModuleId.Value,
				Name = functionality.Name,
				Abbreviation = functionality.Abbreviation,
				FriendlyId = functionality.FriendlyId,
				IconUrl = functionality.IconUrl,
				IsActive = functionality.IsActive,
				IsPartOfMenu = functionality.IsPartOfMenu,
				CreatedAt = functionality.CreatedAt,
				LastUpdatedAt = functionality.LastUpdatedAt,
			};
		}

		public static FunctionalityResponse ToFunctionalityResponse(this Functionality functionality)
		{
			return new FunctionalityResponse
			{
				Id = functionality.Id.Value,
				ModuleId = functionality.ModuleId.Value,
				Name = functionality.Name,
				Abbreviation = functionality.Abbreviation,
				IconUrl = functionality.IconUrl,
				FriendlyId = functionality.FriendlyId,
				IsActive = functionality.IsActive

			};
		}

		public static FunctionalityResponse ToFunctionalityResponseWithModule(this Functionality functionality)
		{
			return new FunctionalityResponse
			{
				Id = functionality.Id.Value,
				ModuleId = functionality.ModuleId.Value,
				Name = functionality.Name,
				Abbreviation = functionality.Abbreviation,
				IconUrl = functionality.IconUrl,
				FriendlyId = functionality.FriendlyId,
				IsActive = functionality.IsActive,
				Module = functionality.Module!.ToModuleResponseWithApplication()
			};
		}

		public static IEnumerable<FunctionalityResponse> ToIEnumerableOfFunctionalityResponse(this IEnumerable<Functionality> functionalities)
		{
			return functionalities.Select(functionality => functionality.ToFunctionalityResponse());
		}

		public static IEnumerable<FunctionalityResponse> ToIEnumerableOfFunctionalityResponseWithModule(this IEnumerable<Functionality> functionalities)
		{
			return functionalities.Select(functionality => functionality.ToFunctionalityResponseWithModule());
		}

	}
}
