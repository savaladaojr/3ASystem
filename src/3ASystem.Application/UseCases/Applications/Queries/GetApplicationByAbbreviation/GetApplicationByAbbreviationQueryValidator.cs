using FluentValidation;

namespace _3ASystem.Application.UseCases.Applications.Queries.GetApplicationByAbbreviation;

public sealed class GetApplicationByAbbreviationQueryValidator : AbstractValidator<GetApplicationByAbbreviationQuery>
{
	public GetApplicationByAbbreviationQueryValidator()
	{
		RuleFor(c => c.Abbreviation).NotEmpty().MaximumLength(25); ;
	}

}
