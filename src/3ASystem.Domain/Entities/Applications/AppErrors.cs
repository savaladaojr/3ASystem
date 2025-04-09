using _3ASystem.Domain.Shared;

namespace _3ASystem.Domain.Entities.Applications;

public static class AppErrors
{
	public static Error NotFound(AppId appId) =>
		Error.NotFound("Applications.NotFound", $"The application with the Id = '{appId.Value}' was not found");

	public static Error Unauthorized() =>
		Error.Failure("Applications.Unauthorized", "You are not authorized to perform this action.");

	public static readonly Error NotFoundByAbbreviation = Error.NotFound(
		"Applications.NotFoundByAbbreviation",
		"The application with the specified abbreviation was not found.");

	public static readonly Error AbbreviationNotUnique = Error.Conflict(
		"Applications.AbbreviationNotUnique",
		"The provided Abbreviation already exist.");

	public static readonly Error FriendlyIdNotUnique = Error.Conflict(
	"Applications.FriendlyIdNotUnique",
	"The provided FriendlyId already exist.");
}
