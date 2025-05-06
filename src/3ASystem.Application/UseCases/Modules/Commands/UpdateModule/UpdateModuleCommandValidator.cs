using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Application.UseCases.Modules.Commands.UpdateModule;
public class UpdateModuleCommandValidator : AbstractValidator<UpdateModuleCommand>
{
	public UpdateModuleCommandValidator()
	{
		RuleFor(p => p.Id).NotEmpty();
		RuleFor(p => p.ApplicationId).NotEmpty();
		RuleFor(p => p.Name).NotEmpty().MaximumLength(100);
		RuleFor(p => p.Abbreviation).NotEmpty().MaximumLength(25);
		RuleFor(p => p.Description).NotEmpty();
		RuleFor(p => p.IconUrl).NotEmpty();
		RuleFor(p => p.FriendlyId).NotEmpty().NotEmpty().MaximumLength(25);
	}

}
