using _3ASystem.Domain.Entities.Applications;

namespace _3ASystem.Application.UseCases.Applications.Responses;

public static class ApplicationResponsesExtentions
{
	public static ApplicationDetailedResponse ToApplicationDetailedResponse(this App app)
	{
		return new ApplicationDetailedResponse
		{
			Id = app.Id.Value,
			Name = app.Name,
			Abbreviation = app.Abbreviation,
			Description = app.Description,
			IconUrl = app.IconUrl,
			Hash = app.Hash,
			IsActive = app.IsActive,
			FriendlyId = app.FriendlyId,
			CreatedAt = app.CreatedAt,
			LastUpdatedAt = app.LastUpdatedAt
		};

	}

	public static ApplicationResponse ToApplicationResponse(this App app)
	{
		return new ApplicationResponse
		{
			Id = app.Id.Value,
			Name = app.Name,
			Abbreviation = app.Abbreviation,
			IconUrl = app.IconUrl,
			IsActive = app.IsActive,
			FriendlyId = app.FriendlyId
		};

	}

	public static IEnumerable<ApplicationResponse> ToIEnumerableOfApplicationResponse(this IEnumerable<App> apps)
	{
		return apps.Select(app => app.ToApplicationResponse());
	}

}
