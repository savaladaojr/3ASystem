﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ASystem.Application.UseCases.Functionalities.Commands.UpdateFunctionality;

public sealed class UpdateFunctionalityCommandValidator : AbstractValidator<UpdateFunctionalityCommand>
{
	public UpdateFunctionalityCommandValidator()
	{
		RuleFor(c => c.Id).NotEmpty();
		RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
		RuleFor(c => c.Abbreviation).NotEmpty().MaximumLength(25);
		RuleFor(c => c.Route).NotEmpty();
		RuleFor(c => c.FriendlyId).NotEmpty().MaximumLength(25);
	}

}
