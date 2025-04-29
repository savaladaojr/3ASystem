using _3ASystem.Domain.Shared;

namespace _3ASystem.Domain.Entities.Modules;

public static class ModuleErrors
{
	public static Error NotFound(ModuleId moduleId) =>
		Error.NotFound("Modules.NotFound", $"The module with the Id = '{moduleId.Value}' was not found");

	public static Error Unauthorized() =>
		Error.Failure("Modules.Unauthorized", "You are not authorized to perform this action.");


	public static readonly Error NotFoundByAbbreviation = Error.NotFound(
		"Modules.NotFoundByAbbreviation",
		"The module with the specified abbreviation was not found."
	);

	public static readonly Error AbbreviationNotUnique = Error.Conflict(
		"Modules.AbbreviationNotUnique",
		"The provided Abbreviation already exist.");

	public static readonly Error FriendlyIdNotUnique = Error.Conflict(
	"Modules.FriendlyIdNotUnique",
	"The provided FriendlyId already exist.");

}
