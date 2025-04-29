using FluentValidation;

namespace _3ASystem.Application.UseCases.Applications.Queries.GetApplicationByFriendlyId;

public sealed class GetApplicationByFriendlyIdQueryValidator : AbstractValidator<GetApplicationByFriendlyIdQuery>
{
	public GetApplicationByFriendlyIdQueryValidator()
	{
		RuleFor(c => c.FriendlyId).NotEmpty().MaximumLength(25); ;
	}

}
