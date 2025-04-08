using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Application.Applications.Commands.UpdateApplication;

public sealed class UpdateApplicationCommandValidator : AbstractValidator<UpdateApplicationCommand>
{
	public UpdateApplicationCommandValidator()
	{
		RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
		RuleFor(c => c.Abbreviation).NotEmpty().MaximumLength(25);
		RuleFor(c => c.Description).NotEmpty();
		RuleFor(c => c.FriendlyId).NotEmpty().MaximumLength(25);
	}

}
