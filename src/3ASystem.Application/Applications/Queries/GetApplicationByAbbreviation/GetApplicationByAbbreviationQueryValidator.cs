using FluentValidation;

namespace _3ASystem.Application.Applications.Queries.GetApplicationById;

public sealed class GetApplicationByAbbreviationQueryValidator : AbstractValidator<GetApplicationByAbbreviationQuery>
{
	public GetApplicationByAbbreviationQueryValidator()
	{
		RuleFor(c => c.Abbreviation).NotEmpty().MaximumLength(25); ;
	}

}
