using FluentValidation;

namespace _3ASystem.Application.UseCases.Applications.Queries.GetApplicationByHash;

public sealed class GetApplicationByHashQueryValidator : AbstractValidator<GetApplicationByHashQuery>
{
	public GetApplicationByHashQueryValidator()
	{
		RuleFor(c => c.Hash).NotEqual(Guid.Empty);
	}

}
