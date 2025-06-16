using _3ASystem.Domain.Shared;

namespace _3ASystem.Domain.Entities.Functionalities;

public static class FunctionalityErrors
{
	public static Error NotFound(FunctionalityId functionalityId) =>
		Error.NotFound("Functionalities.NotFound", $"The functionality with the Id = '{functionalityId.Value}' was not found");

	public static Error Unauthorized() =>
		Error.Failure("Functionalities.Unauthorized", "You are not authorized to perform this action.");


	public static readonly Error NotFoundByAbbreviation = Error.NotFound(
		"Functionalities.NotFoundByAbbreviation",
		"The functionality with the specified abbreviation was not found."
	);

	public static readonly Error AbbreviationNotUnique = Error.Conflict(
		"Functionalities.AbbreviationNotUnique",
		"The provided Abbreviation already exist.");

	public static readonly Error FriendlyIdNotUnique = Error.Conflict(
	"Functionalities.FriendlyIdNotUnique",
	"The provided FriendlyId already exist.");

}
