using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Application.UseCases.Functionalities.Commands.CreateFunctionality;
public class CreateFunctionalityCommandValidator : AbstractValidator<CreateFunctionalityCommand>
{
	public CreateFunctionalityCommandValidator()
	{
		RuleFor(p => p.ModuleId).NotEmpty();
		RuleFor(p => p.Name).NotEmpty().MaximumLength(100);
		RuleFor(p => p.Abbreviation).NotEmpty().MaximumLength(25);
		RuleFor(p => p.Route).NotEmpty();
		RuleFor(p => p.FriendlyId).NotEmpty().MaximumLength(25);
	}

}
